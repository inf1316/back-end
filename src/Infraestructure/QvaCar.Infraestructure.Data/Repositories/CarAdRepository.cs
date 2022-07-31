using Microsoft.EntityFrameworkCore;
using QvaCar.Domain.CarAds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class CarAdRepository : ICarAdRepository
    {
        private readonly QvaCarDbContext _dbContext;

        public CarAdRepository(QvaCarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CarAd aggregateRoot, CancellationToken cancellationToken)
        {
            _dbContext.Ads.Add(aggregateRoot);           
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<CarAd?> GetByUserAndAdIdsAsync(Guid userId, Guid adId, CancellationToken cancellationToken)
        {                       
            return  await _dbContext.Ads.WithPartitionKey(userId.ToString()).FirstOrDefaultAsync(x => x.Id == adId, cancellationToken);
        }

        public async Task UpdateAsync(CarAd aggregateRoot, CancellationToken cancellationToken)
        {
            _dbContext.Ads.Update(aggregateRoot);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
