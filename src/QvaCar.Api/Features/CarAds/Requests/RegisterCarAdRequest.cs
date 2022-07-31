using System;

namespace QvaCar.Api.Features.CarAds
{
    public record RegisterCarAdRequest
    {
        public long Price { get; init; }
        public int ProvinceId { get; init; }
        public int ManufacturingYear { get; init; }
        public int Kilometers { get; init; }
        public int BodyTypeId { get; init; }
        public int ColorId { get; init; }
        public int FuelTypeId { get; init; }
        public int GearboxTypeId { get; init; }
        public int[] ExteriorTypeIds { get; init; } = Array.Empty<int>();
        public int[] SafetyTypeIds{ get; init; } = Array.Empty<int>();
        public int[] InsideTypeIds { get; init; } = Array.Empty<int>();
        public string Description { get; init; } = string.Empty;
        public string ContactPhoneNumber { get; init; } = string.Empty;
        public string ModelVersion { get; init; } = string.Empty;
        public RegisterCarAdCoordinate? ContactLocation { get; init; }
    }

    public record RegisterCarAdCoordinate
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
