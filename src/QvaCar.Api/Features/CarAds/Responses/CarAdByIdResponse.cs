using System;
using System.Collections.Generic;

namespace QvaCar.Api.Features.CarAds
{
    public record CarAdByIdResponse
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

        public CarAdByIdCoordinateResponse? ContactLocation { get; init; }

        public BaseItemByIdResponse[] ExteriorTypes { get; init; } = Array.Empty<BaseItemByIdResponse>();
        public BaseItemByIdResponse[] SafetyTypes { get; init; } = Array.Empty<BaseItemByIdResponse>();
        
        public BaseItemByIdResponse[] InsideTypes { get; init; } = Array.Empty<BaseItemByIdResponse>();
        public List<CarAdImagebByIdResponse> Images { get; init; } = new List<CarAdImagebByIdResponse>();
    }

    public record CarAdImagebByIdResponse
    {
        public string FileName { get; init; } = string.Empty;
        public IList<CarAdImageResponse> Urls { get; init; } = new List<CarAdImageResponse>();
    }

    public record CarAdImageResponse
    {
        public string Size { get; init; } = string.Empty;
        public Uri Url { get; init; }
    }

    public record BaseItemByIdResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    public record CarAdByIdCoordinateResponse
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
