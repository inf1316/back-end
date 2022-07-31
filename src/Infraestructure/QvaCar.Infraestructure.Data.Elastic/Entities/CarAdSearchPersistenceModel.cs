using Nest;
using QvaCar.Domain.Search;
using System;
using System.Collections.Generic;

namespace QvaCar.Infraestructure.Data.Elastic.Entities
{
    public class CarAdSearchPersistenceModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;
        public long Price { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; } = string.Empty;
        public int ManufacturingYear { get; set; }
        public int Kilometers { get; set; }
        public int BodyTypeId { get; set; }
        public string BodyTypeName { get; set; } = string.Empty;
        public int ColorId { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public int FuelTypeId { get; set; }
        public string FuelTypeName { get; set; } = string.Empty;
        public int GearboxTypeId { get; set; }
        public string GearboxTypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactPhoneNumber { get; set; } = string.Empty;
        public string ModelVersion { get; set; } = string.Empty;
        public List<int> ExteriorTypesIds { get; set; } = new List<int>();
        public IReadOnlyCollection<ExteriorTypeSearchModel> ExteriorTypes { get; set; } = new List<ExteriorTypeSearchModel>();

        public List<int> InsideTypesIds { get; set; } = new List<int>();
        public IReadOnlyCollection<InsideTypeSearchModel> InsideTypes { get; set; } = new List<InsideTypeSearchModel>();

        public List<int> SafetyTypesIds { get; set; } = new List<int>();
        public IReadOnlyCollection<SafetyTypeSearchModel> SafetyTypes { get; set; } = new List<SafetyTypeSearchModel>();

        public List<string> Images { get; set; } = new List<string>();

        public GeoLocation? ContactLocation { get; set; }
        public CarAdSearchPersistenceModel() { }
    }
}
