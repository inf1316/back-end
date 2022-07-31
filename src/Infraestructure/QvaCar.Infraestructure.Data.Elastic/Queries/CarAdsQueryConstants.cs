using QvaCar.Domain.Search;
using QvaCar.Infraestructure.Data.Elastic.Entities;

namespace QvaCar.Infraestructure.Data.Elastic.Queries
{
    internal class CarAdsQueryConstants
    {
        public const string ProvinceAggName = nameof(CarAdSearchPersistenceModel.ProvinceId);
        public const string BodyTypeAggName = nameof(CarAdSearchPersistenceModel.BodyTypeId);
        public const string ColorAggName = nameof(CarAdSearchPersistenceModel.ColorId);
        public const string FuelTypeAggName = nameof(CarAdSearchPersistenceModel.FuelTypeId);
        public const string GearboxTypeAggName = nameof(CarAdSearchPersistenceModel.GearboxTypeId);
        public const string DistanceFromLocationScriptFieldName = "DistanceFromContactLocation";

        public const string ExteriorTypesIdsAggName = nameof(CarAdSearchPersistenceModel.ExteriorTypesIds);
        public const string InsideTypesIdsAggName = nameof(CarAdSearchPersistenceModel.InsideTypesIds);
        public const string SafetyTypesAggName = nameof(CarAdSearchPersistenceModel.SafetyTypes);
    }
}