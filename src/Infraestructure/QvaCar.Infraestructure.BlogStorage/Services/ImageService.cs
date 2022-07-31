using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using ImageMagick;
using LazZiya.ImageResize;
using MimeTypes;
using QvaCar.Domain.CarAds.Services;
using QvaCar.Infraestructure.BlogStorage.Configuration.Options;
using QvaCar.Infraestructure.BlogStorage.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DomainNamespace = QvaCar.Domain.CarAds;

namespace QvaCar.Infraestructure.BlogStorage.Services
{
    internal class ImageService : IImageService
    {
        private readonly ImageServiceOptions _imageOption;
        private readonly BlobContainerClient _blobContainerClient;

        public ImageService
            (
                ImageServiceOptions imageOption,
                BlobContainerClient blobServiceClient
            )
        {
            _imageOption = imageOption;
            _blobContainerClient = blobServiceClient;
        }

        public List<ImageVersionResponse> GetUrlsForImage(Guid userId, Guid adId, string fileNameWithExtension)
        {
            var versions = new List<ImageVersionResponse>()
            {
                new ImageVersionResponse( DomainNamespace.ImageType.Small, GetBlobReadLocationUrlForImage(userId, adId, fileNameWithExtension,DomainNamespace.ImageType.Small)),
                new ImageVersionResponse( DomainNamespace.ImageType.Medium, GetBlobReadLocationUrlForImage(userId, adId, fileNameWithExtension,DomainNamespace.ImageType.Medium)),
                new ImageVersionResponse( DomainNamespace.ImageType.Large, GetBlobReadLocationUrlForImage(userId, adId, fileNameWithExtension,DomainNamespace.ImageType.Large)),
            };
            return versions;
        }

        public async Task SaveImageAsync(Guid userId, Guid adId, string fileNameWithExtension, byte[] imageData, CancellationToken cancellationToken)
        {
            var versions = new[] { DomainNamespace.ImageType.Small, DomainNamespace.ImageType.Medium, DomainNamespace.ImageType.Large };
            var versionTasks = new List<Task>();

            foreach (var version in versions)
            {
                var blobClient = GetBlobClientForImage(userId, adId, fileNameWithExtension, version.Name);

                var fileExtension = Path.GetExtension(fileNameWithExtension);
                var contentType = MimeTypeMap.GetMimeType(fileExtension);

                (int width, int height) = GetDimensionByVersion(version.Name);

                versionTasks.Add(CreateAndUpdloadImage(width, height, blobClient, contentType, imageData, fileExtension, cancellationToken));
            }
            await Task.WhenAll(versionTasks);
        }

        private Uri GetBlobReadLocationUrlForImage(Guid userId, Guid adId, string fileName, DomainNamespace.ImageType imageType)
        {
            string baseAddr = _imageOption.BaseUrl;
            string fileLocation = $"{ _imageOption.ContainerName}/{ userId}/{adId}";
            string realFileName = $"{ Path.GetFileNameWithoutExtension(fileName) }_{imageType.Name.ToLower()}{ Path.GetExtension(fileName) }";
            var builder = new UriBuilder(new Uri(baseAddr));
            var endsWithSlash = builder.Path.EndsWith("/");
            builder.Path += $"{(endsWithSlash?"":"/")}{fileLocation}/{realFileName}";
            return builder.Uri;
        }

        private BlockBlobClient GetBlobClientForImage(Guid userId, Guid adId, string imageName, string version)
        {
            var fileName = $"{ Path.GetFileNameWithoutExtension(imageName) }_{ version.ToLower() }{ Path.GetExtension(imageName) }";
            var filePath = Path.Combine(userId.ToString(), adId.ToString(), fileName);
            return _blobContainerClient.GetBlockBlobClient(filePath);
        }

        private static (int Width, int Height) GetDimensionByVersion(string version)
        {
            (int Width, int Height) Dimensions = default;
            switch (version)
            {
                case "Small":
                    {
                        Dimensions = (300, 300);
                    }
                    break;
                case "Medium":
                    {
                        Dimensions = (500, 500);
                    }
                    break;
                case "Large":
                    {
                        Dimensions = (1000, 1000);
                    }
                    break;
                default:
                    throw new VersionFailException("Unsupported Version");
            }
            return Dimensions;
        }

        private static async Task CreateAndUpdloadImage(int width, int height, BlockBlobClient blobClient,
                                                           string contentType, byte[] image, string fileExtension,
                                                           CancellationToken cancellationToken)
        {
            using (var saveStream = new MemoryStream())
            {
                ProccessImageToStream(width, height, fileExtension, saveStream, image);
                var opt = new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    }
                };
                await blobClient.UploadAsync(saveStream, opt, cancellationToken);
                saveStream.Position = 0;
            }
        }

        private static void ProccessImageToStream(int width, int height, string fileExtension, MemoryStream saveStream, byte[] image)
        {
            using (var imageForStream = new MemoryStream(image))
            using (var imageCopy = new MemoryStream())
            {
                imageForStream.CopyTo(imageCopy);
                imageCopy.Position = 0;

                imageForStream.Position = 0;
                var stream = AddImageWatterMark(imageCopy, fileExtension);

                using (var magickImage = new MagickImage(stream))
                {
                    if (magickImage.Height != height || magickImage.Width != width)
                    {
                        double ratio = (double)height / magickImage.Height;

                        int newWidth = (int)(magickImage.Width * ratio);
                        int newHeight = (int)(magickImage.Height * ratio);

                        magickImage.Resize(newWidth, newHeight);
                    }
                    magickImage.Write(saveStream);
                }
                saveStream.Position = 0;
            }
        }

        private static Stream AddImageWatterMark(Stream file, string fileExtension)
        {
            var stream = new MemoryStream();
            using (var image = Image.FromStream(file))
            {
                var waterMarkOptions = new TextWatermarkOptions
                {
                    TextColor = Color.FromArgb(50, Color.White),
                    OutlineColor = Color.FromArgb(255, Color.Black),
                    Location = TargetSpot.BottomRight
                };
                switch (fileExtension.ToLower())
                {
                    case ".jpeg":
                        {
                            image.AddTextWatermark("QvaCar", waterMarkOptions).Save(stream, ImageFormat.Jpeg);
                        }
                        break;
                    case ".png":
                        {
                            image.AddTextWatermark("QvaCar", waterMarkOptions).Save(stream, ImageFormat.Png);
                        }
                        break;
                    case ".jpg":
                        {
                            image.AddTextWatermark("QvaCar", waterMarkOptions).Save(stream, ImageFormat.Jpeg);
                        }
                        break;
                    default:
                        throw new ExtensionFailException("Unknown Extension");
                }
            }
            stream.Position = 0;
            return stream;
        }
    }
}