using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Api.FunctionalTests.SeedWork;
using QvaCar.Api.FunctionalTests.Shared.CarAds;
using QvaCar.Api.FunctionalTests.Shared.CarAds.Extensions;
using QvaCar.Api.FunctionalTests.Shared.Identity;
using QvaCar.Domain.CarAds;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Color = System.Drawing.Color;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    [Collection(nameof(TestServerFixtureCollection))]
    public class WhenAddingImagesToCarAds
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;

        public WhenAddingImagesToCarAds(TestServerFixture given)
        {
            Given = given ?? throw new Exception("Null Server");
            ValidUser = new TestApiUserBuilder()
                .WithId(Guid.NewGuid())
                .WithFreeSubscriptionLevel()
                .Build();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Unauthorized_When_User_Is_Not_Logged_In()
        {
            var state = AdState.Draft;
            string fileName = "Images.gifts";
            string mimeType = "image/gifts";

            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state);

            var fileContent = CreateImageInMemory();
            var response = await PostImageAsAnonymousAsync(carInDb.Id, new FileUploadDescriptor(fileName, mimeType, fileContent));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Bad_Request_When_Request_Is_Invalid()
        {
            var state = AdState.Draft;
            string fileName = "Images.gifts";
            string mimeType = "image/gifts";

            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state);

            var fileContent = CreateImageInMemory();
            var response = await PostImageAsync(ValidUser, carInDb.Id, new FileUploadDescriptor(fileName, mimeType, fileContent));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Upload_Single_Image()
        {
            var state = AdState.Draft;
            string fileName = "Images.jpeg";
            string mimeType = "image/jpeg";

            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state);

            var fileContent = CreateImageInMemory();
            var response = await PostImageAsync(ValidUser, carInDb.Id, new FileUploadDescriptor(fileName, mimeType, fileContent));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);
            var images = carAdsInDb.First().Images;
            images.Should().NotBeNull().And.HaveCount(1);
            var image = images.First();
            image.FileName.Should().Be(fileName);
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Upload_Multiple_Images_At_Once()
        {
            var state = AdState.Draft;
            string firstImageFileName = "Images.jpeg";
            string firstImageType = "image/jpeg";
            var firstImageContent = CreateImageInMemory();
            var fistImage = new FileUploadDescriptor(firstImageFileName, firstImageType, firstImageContent);

            string secondImageFileName = "Images1.jpeg";
            string secondImageType = "image/jpeg";
            var secondImageContent = CreateImageInMemory();
            var secondImage = new FileUploadDescriptor(secondImageFileName, secondImageType, secondImageContent);

            var expectedFileNames = new string[] { firstImageFileName, secondImageFileName };

            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state);

            var response = await PostImageAsync(ValidUser, carInDb.Id, new[] { fistImage, secondImage });
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);
            var images = carAdsInDb.First().Images;
            images.Should().NotBeNull().And.HaveCount(2);
            var fileNames = images.Select(x => x.FileName);
            fileNames.Should().BeEquivalentTo(expectedFileNames);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_UploadingImages_To_Another_User_Ad()
        {
            var invalidUserId = Guid.NewGuid();
            var state = AdState.Draft;
            string fileName = "Images.jpeg";
            string mimeType = "image/jpeg";

            var carInDb = await GivenDefaultAdInRepositoryForUser(invalidUserId, state);

            var fileContent = CreateImageInMemory();
            var imageToUpload = new FileUploadDescriptor(fileName, mimeType, fileContent);
            var response = await PostImageAsync(ValidUser, carInDb.Id, new[] { imageToUpload, imageToUpload });
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseModel = response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Upload_Multiple_Images_With_Same_Name()
        {
            var state = AdState.Draft;
            string fileName = "Images.jpeg";
            string mimeType = "image/jpeg";

            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state);

            var expectedFileNames = new string[] { fileName, "Images1.jpeg" };
            var fileContent = CreateImageInMemory();
            var imageToUpload = new FileUploadDescriptor(fileName, mimeType, fileContent);
            var response = await PostImageAsync(ValidUser, carInDb.Id, new[] { imageToUpload, imageToUpload });
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);
            var images = carAdsInDb.First().Images;
            images.Should().NotBeNull().And.HaveCount(2);
            var fileNames = images.Select(x => x.FileName);
            fileNames.Should().BeEquivalentTo(expectedFileNames);
        }

        #region Helpers       
        private static byte[] CreateImageInMemory()
        {
            using (Bitmap bitmap = new(1000, 800, PixelFormat.Format32bppPArgb))
            using (var graphics = Graphics.FromImage(bitmap))
            using (Pen drawing = new(Color.FromKnownColor(KnownColor.Blue), 2))
            {
                graphics.DrawArc(drawing, 0, 0, 700, 700, 0, 180);
                using (Pen drawing_1 = new(Color.FromKnownColor(KnownColor.Red), 2))
                {
                    graphics.DrawEllipse(drawing_1, 10, 10, 900, 700);
                    using (var _imageCopy = new MemoryStream())
                    {
                        bitmap.Save(_imageCopy, ImageFormat.Jpeg);
                        _imageCopy.Position = 0;
                        return _imageCopy.ToArray();
                    }
                }
            }
        }

        private async Task<CarAd> GivenDefaultAdInRepositoryForUser(Guid userId, AdState state, string[]? images = null)
        {
            var priceInDollars = 1500;
            var province = Province.SantiagoDeCuba;
            var manufacturingYear = 1991;
            var kilometers = 101;
            var bodyType = CarBodyType.Familiar;
            var color = Domain.CarAds.Color.Negro;
            var fuelType = FuelType.Gasolina;
            var gearboxType = GearboxType.Automatico;
            var description = "Some description 2";
            var contactPhoneNumber = "551012";
            var version = "Yaris";
            var time = new DateTime(2020, 01, 01, 12, 20, 01);
            var updateTime = new DateTime(2020, 01, 01, 12, 20, 01);

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var carInDb = new CarAdBuilder()
                                .WithAdState(state)
                                .WithCreatedAt(time)
                                .WithUpdateAt(updateTime)
                                .WithUserId(userId)
                                .WithPrice(priceInDollars)
                                .WithProvince(province)
                                .WithManufacturingYear(manufacturingYear)
                                .WithKilometers(kilometers)
                                .WithBodyType(bodyType)
                                .WithColor(color)
                                .WithFuelType(fuelType)
                                .WithGearboxType(gearboxType)
                                .WithDescription(description)
                                .WithContactPhoneNumber(contactPhoneNumber)
                                .WithModelVersion(version)
                                .WithImages(images ?? Array.Empty<string>())
                                .WithExteriorTypes(exteriorTypes)
                                .WithInsideTypes(insideTypes)
                                .WithSafetyTypes(safetyTypes)
                                .Build();
            await Given.AssumeCarAdInRepository(carInDb);
            return carInDb;
        }
        private async Task<HttpResponseMessage> PostImageAsAnonymousAsync(Guid adId, params FileUploadDescriptor[] files)
        {
            return await PostImageAsync(null, adId, files);
        }

        private async Task<HttpResponseMessage> PostImageAsync(TestApiUser? asUser, Guid adId, params FileUploadDescriptor[] files)
        {
            var formData = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var content = new ByteArrayContent(file.FileContent);
                content.Headers.ContentType = new MediaTypeHeaderValue(file.MimeType);
                formData.Add(content, "Images", file.FileName);
            }

            var postURl = $"{ ApiHelper.Post.RegisterImageCarAdUrl(adId) }";
            var resquest = new HttpRequestMessage(HttpMethod.Post, postURl)
            {
                Content = formData,
            };

            var client = Given.Server.CreateClient();
            if (asUser is not null)
                client.FromUser(asUser);
            return await client.SendAsync(resquest);
        }

        private class FileUploadDescriptor
        {
            public string FileName { get; set; } = string.Empty;
            public string MimeType { get; set; } = string.Empty;
            public byte[] FileContent { get; set; }
            public FileUploadDescriptor(string fileName, string mimeType, byte[] fileContent)
            {
                FileName = fileName;
                MimeType = mimeType;
                FileContent = fileContent;
            }
        }

        private static class ApiHelper
        {
            public static class Post
            {
                public static string RegisterImageCarAdUrl(Guid id)
                {
                    return $"/api/car-ads/{ id }/images/upload";
                }
            }
        }
        #endregion Helpers
    }
}
