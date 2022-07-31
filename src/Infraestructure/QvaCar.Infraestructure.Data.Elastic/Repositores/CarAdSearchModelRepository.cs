using AutoMapper;
using Nest;
using QvaCar.Domain.Search;
using QvaCar.Infraestructure.Data.Elastic.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data.Elastic.Repositores
{
    internal class CarAdSearchModelRepository : ICarAdSearchModelRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public CarAdSearchModelRepository(IElasticClient elasticClient, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;

        }
        public async Task AddAsync(CarAdSearchModel aggregateRoot, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<CarAdSearchPersistenceModel>(aggregateRoot);
            await _elasticClient.IndexDocumentAsync(entity, ct: cancellationToken);
        }

        public async Task DeleteIfExistAsync(Guid id, CancellationToken cancellationToken)
        {
            await _elasticClient.DeleteAsync<CarAdSearchPersistenceModel>(id, ct: cancellationToken);
        }

        public async Task<CarAdSearchModel?> GetByIdOrDefaultAsync(Guid adId, CancellationToken cancellationToken)
        {

            var response = await _elasticClient.GetAsync<CarAdSearchPersistenceModel>(adId, ct: cancellationToken);
            if (!response.Found)
                return null;
            var entity = _mapper.Map<CarAdSearchModel>(response.Source);
            return entity;
        }

        public async Task UpdateAsync(CarAdSearchModel aggregateRoot, CancellationToken cancellationToken)
        {
            await AddAsync(aggregateRoot, cancellationToken);
        }
    }
}