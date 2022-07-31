using System;
using System.Collections.Generic;

namespace QvaCar.Application.Features.CarAds
{
    public record GetCarAdsByUserResponse
    {
        public IReadOnlyList<CarAdByUserItemResponse> Ads { get; init; } = new List<CarAdByUserItemResponse>();
    }

    public record CarAdByUserItemResponse
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

        public CarAdByUserListCoordinateResponse? ContactLocation { get; init; }


        public BaseItemByUserResponse[] ExteriorTypes { get; init; } = Array.Empty<BaseItemByUserResponse>();
        public BaseItemByUserResponse[] SafetyTypes { get; init; } = Array.Empty<BaseItemByUserResponse>();
        public BaseItemByUserResponse[] InsideTypes { get; init; } = Array.Empty<BaseItemByUserResponse>();

        public IList<GetCarAdsImageByUserResponse> Images { get; init; } = new List<GetCarAdsImageByUserResponse>();
    }    

    public record GetCarAdsImageByUserResponse
    {
        public string FileName { get; init; } = string.Empty;
        public List<ImageByUserResponse> Urls { get; init; } = new List<ImageByUserResponse>();
    }

    public record ImageByUserResponse
    {
        public string Size { get; init; } = string.Empty;
        public Uri Url { get; init; }
    }

    public record BaseItemByUserResponse 
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    public record CarAdByUserListCoordinateResponse
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
