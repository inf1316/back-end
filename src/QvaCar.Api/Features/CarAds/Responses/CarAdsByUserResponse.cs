using System;
using System.Collections.Generic;

namespace QvaCar.Api.Features.CarAds
{
    public record CarAdsResponse
    {
        public IEnumerable<CarAdListItemResponse> Ads { get; init; } = new List<CarAdListItemResponse>();
    }

    public record CarAdListItemResponse
    {
        public Guid UserId { get; init; }
        public string Id { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }

        public int StateId { get; init; }
        public string StateName { get; init; } = string.Empty;

        public long Price { get; init; }
        public int ManufacturingYear { get; init; }
        public int Kilometers { get; init; }

        public string Description { get; init; } = string.Empty;
        public string ContactPhoneNumber { get; init; } = string.Empty;
        public string ModelVersion { get; init; } = string.Empty;

        public int ProvinceId { get; init; }
        public string ProvinceName { get; init; } = string.Empty;

        public int BodyTypeId { get; init; }
        public string BodyTypeName { get; init; } = string.Empty;

        public int ColorId { get; init; }
        public string ColorName { get; init; } = string.Empty;

        public int FuelTypeId { get; init; }
        public string FuelTypeName { get; init; } = string.Empty;

        public int GearboxTypeId { get; init; }
        public string GearboxTypeName { get; init; } = string.Empty;

        public CarAdByUserCoordinateResponse? ContactLocation { get; init; }

        public BaseItemListResponse[] ExteriorTypes { get; init; } = Array.Empty<BaseItemListResponse>();
        public BaseItemListResponse[] SafetyTypes { get; init; } = Array.Empty<BaseItemListResponse>();
        public BaseItemListResponse[] InsideTypes { get; init; } = Array.Empty<BaseItemListResponse>();

        public List<CarAdListImageResponse> Images { get; init; } = new List<CarAdListImageResponse>();
    }

    public record CarAdListImageResponse
    {
        public string FileName { get; init; } = string.Empty;
        public List<CarAdImage> Urls { get; init; } = new List<CarAdImage>();
    }

    public record CarAdImage
    {
        public string Size { get; init; } = string.Empty;
        public Uri Url { get; init; }
    }

    public record BaseItemListResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    public record CarAdByUserCoordinateResponse
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
