using QvaCar.Seedwork.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Domain.CarAds
{
    public interface ICarAdRepository : IAggregateRepository<CarAd, Guid>
    {
        Task<CarAd?> GetByUserAndAdIdsAsync(Guid userId, Guid carId, CancellationToken cancellationToken);
    }
}