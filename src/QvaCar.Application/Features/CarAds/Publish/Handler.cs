using MediatR;
using QvaCar.Application.Exceptions;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using QvaCar.Seedwork.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.CarAds
{
    public class PublishCarAdByIdCommandHandler : IRequestHandler<PublishCarAdByIdCommand>
    {
        private readonly ICarAdRepository _carAdRepository;
        private readonly IClockService _clockService;
        private readonly ICurrentUserService _currentUserService;

        public PublishCarAdByIdCommandHandler
            (
                ICarAdRepository carAdRepository,
                IClockService clockService,
                ICurrentUserService currentUserService
            )
        {
            this._carAdRepository = carAdRepository;
            this._clockService = clockService;
            this._currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(PublishCarAdByIdCommand command, CancellationToken cancellationToken)
        {
            var user = _currentUserService.GetCurrentUser();
            var userId = user.Id;

            var carAd = await _carAdRepository.GetByUserAndAdIdsAsync(userId, command.Id, cancellationToken);
            Check.IsNotNull<EntityNotFoundException>(carAd, $"Entity with Id {command.Id} not found.");

            carAd.Publish(_clockService.Now);

            await _carAdRepository.UpdateAsync(carAd,cancellationToken);

            return Unit.Value;
        }
    }
}
