using System.Collections.Generic;

namespace QvaCar.Api.Features.CarAds
{
    public record CarAdReferenceDataResponse
    {
        public IEnumerable<BaseReferenceDataItemResponse> States { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> Provinces { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> CarBodyTypes { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> Colors { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> FuelTypes { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> GearboxTypes { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> ExteriorTypes { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> InsideTypes { get; init; } = new List<BaseReferenceDataItemResponse>();
        public IEnumerable<BaseReferenceDataItemResponse> SafetyTypes { get; init; } = new List<BaseReferenceDataItemResponse>();
    }

    public record BaseReferenceDataItemResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
