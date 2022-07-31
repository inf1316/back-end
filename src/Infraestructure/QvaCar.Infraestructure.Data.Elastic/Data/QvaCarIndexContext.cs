using Microsoft.Extensions.Options;
using Nest;
using QvaCar.Infraestructure.Data.Elastic.Configuration.Options;
using QvaCar.Infraestructure.Data.Elastic.Entities;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data.Elastic
{
    public class QvaCarIndexContext
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchConfiguration _options;

        public IElasticClient DatabaseClient => _client;
        public string CarAdsIndexName => _options.CarAdsIndexName;

        public QvaCarIndexContext(IElasticClient client, IOptions<ElasticSearchConfiguration> options)
        {
            _client = client;
            _options = options.Value;
        }

        public static void AddQvaCarDefaultMappings(ConnectionSettings connectionSettings)
        {
            connectionSettings
                .DefaultMappingFor<CarAdSearchPersistenceModel>(map => map
                    .IdProperty(model => model.Id)
                );
        }

        public async Task CreateIndexIfNotExistAsync()
        {
            var response = await _client.Indices.ExistsAsync(_options.CarAdsIndexName);

            if (response.Exists)
                return;

            var createResponse = await _client.Indices.CreateAsync(_options.CarAdsIndexName, index =>
            {
                index
                    .Settings(se => se.NumberOfReplicas(1).NumberOfShards(1));
                MapAdSearchModel(index);
                return index;
            });
        }

        public async Task<bool> EnsureDeletedIndexAsync()
        {
            var response = await _client.Indices.ExistsAsync(_options.CarAdsIndexName);

            if (!response.Exists)
                return true;

            var deleteResponse = await _client.Indices.DeleteAsync(_options.CarAdsIndexName);
            return deleteResponse.Acknowledged;
        }

        public async Task<bool> ClearIndexAsync()
        {
            var deleteResponse = await _client.DeleteByQueryAsync<CarAdSearchPersistenceModel>(q => q.Query(rq => rq.MatchAll()));
            return deleteResponse.IsValid;
        }

        private static void MapAdSearchModel(CreateIndexDescriptor index)
        {
            index.Map<CarAdSearchPersistenceModel>(m => m
             .AutoMap<CarAdSearchPersistenceModel>()
             .Properties(prop => prop
                 .Keyword(k => k.Name(model => model.StateId))
                 .Keyword(k => k.Name(model => model.StateName).Index(false))
                 .Keyword(k => k.Name(model => model.ProvinceId))
                 .Keyword(k => k.Name(model => model.ProvinceName).Index(false))
                 .Keyword(k => k.Name(model => model.ColorId))
                 .Keyword(k => k.Name(model => model.ColorName).Index(false))
                 .Keyword(k => k.Name(model => model.BodyTypeId))
                 .Keyword(k => k.Name(model => model.BodyTypeName).Index(false))
                 .Keyword(k => k.Name(model => model.FuelTypeId))
                 .Keyword(k => k.Name(model => model.FuelTypeName).Index(false))
                 .Keyword(k => k.Name(model => model.GearboxTypeId))
                 .Keyword(k => k.Name(model => model.GearboxTypeName).Index(false))
                 .Keyword(k => k.Name(model => model.ContactPhoneNumber).Index(false))
                 .Keyword(k => k.Name(model => model.ExteriorTypesIds))
                 .Flattened(k => k.Name(model => model.ExteriorTypes).Index(false))
                 .Keyword(k => k.Name(model => model.InsideTypesIds))
                 .Flattened(k => k.Name(model => model.InsideTypes).Index(false))
                 .Keyword(k => k.Name(model => model.SafetyTypesIds))
                 .Flattened(k => k.Name(model => model.SafetyTypes).Index(false))
                 .Keyword(k => k.Name(model => model.Images).Index(false))
             )
             .Dynamic(DynamicMapping.Strict)
            );
        }
    }
}
