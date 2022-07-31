using System;

namespace QvaCar.Domain.CarAds
{
    public record GetCarAdListItemQueryResponse
    {
        public Guid UserId { get; init; }
        public Guid Id { get; init; }

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

        public CarAdByUserCoordinateQueryResponse? ContactLocation { get; init; }

        public BaseItemListQueryResponse[] ExteriorTypes { get; init; } = Array.Empty<BaseItemListQueryResponse>();
        public BaseItemListQueryResponse[] SafetyTypes { get; init; } = Array.Empty<BaseItemListQueryResponse>();
        public BaseItemListQueryResponse[] InsideTypes { get; init; } = Array.Empty<BaseItemListQueryResponse>();

        public string[] Images { get; init; } = Array.Empty<string>();
    }

    public record BaseItemListQueryResponse 
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    public record CarAdByUserCoordinateQueryResponse
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
