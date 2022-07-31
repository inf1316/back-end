using System.Collections.Generic;

namespace QvaCar.Application.Features.CarAds
{
    public record GetCarAdsReferenceDataResponse
    {
        public IReadOnlyList<BaseReferenceDataItem> States { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> Provinces { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> CarBodyTypes { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> Colors { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> FuelTypes { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> GearboxTypes { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> ExteriorTypes { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> InsideTypes { get; init; } = new List<BaseReferenceDataItem>();
        public IReadOnlyList<BaseReferenceDataItem> SafetyTypes { get; init; } = new List<BaseReferenceDataItem>();
    }

    public record BaseReferenceDataItem
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
