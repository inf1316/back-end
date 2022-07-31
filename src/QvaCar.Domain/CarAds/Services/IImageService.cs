using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Domain.CarAds.Services
{

    public interface IImageService
    {
        List<ImageVersionResponse> GetUrlsForImage(Guid userId, Guid adId, string fileNameWithExtension);
        Task SaveImageAsync(Guid userId, Guid adId, string fileNameWithExtension, byte[] imageData, CancellationToken cancellationToken);
    }
}
