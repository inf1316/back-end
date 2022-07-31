using QvaCar.Domain.CarAds;
using QvaCar.Domain.Chat;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace QvaCar.Infraestructure.Chat.Queries
{
    internal class ChatCarAdQueries : IChatCarAdQueries
    {
        private readonly ICarAdsQueries _carAdQueries;

        public ChatCarAdQueries(ICarAdsQueries carAdQueries)
        {
            _carAdQueries = carAdQueries;
        }

        public async Task<ChatAdByIdQueryResponse?> GetByIdOrDefaultAsync(Guid userId, Guid adId, CancellationToken cancellationToken)
        {
            var adResponse = await _carAdQueries.GetCarAdByUserAndAdIdAsync(userId, adId, cancellationToken);

            if (adResponse is null) return null;

            return new ChatAdByIdQueryResponse()
            {
                Id = adResponse.Id,
                OwnerId = adResponse.UserId,
                ModelVersion = adResponse.ModelVersion,
                ImageFileName = adResponse.Images.FirstOrDefault(),
            };
        }
    }
}
