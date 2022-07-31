using System.Collections.Generic;

namespace QvaCar.Application.Features.Identity
{
    public record ViewLoginQueryResponse
    {
        public IReadOnlyList<BaseReferenceDataItem> Provinces { get; init; } = new List<BaseReferenceDataItem>();
    }

    public record BaseReferenceDataItem
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}