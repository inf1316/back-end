using System;

namespace QvaCar.Domain.CarAds.Services
{
    public record ImageVersionResponse
    {
        public ImageType Type { get; init; }
        public Uri Url { get; init; }

        public ImageVersionResponse(ImageType type, Uri url)
        {
            Type = type;
            Url = url;
        }
    }
}