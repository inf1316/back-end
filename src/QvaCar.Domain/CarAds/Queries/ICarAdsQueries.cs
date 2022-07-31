using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Domain.CarAds
{
    public interface ICarAdsQueries
    {
        public Task<IReadOnlyList<GetCarAdListItemQueryResponse>> GetCarAdsByUserAsync(Guid userId, CancellationToken cancellationToken);
        public Task<GetCarAdByIdQueryResponse?> GetCarAdByUserAndAdIdAsync(Guid userId, Guid adId, CancellationToken cancellationToken);
    }
}
