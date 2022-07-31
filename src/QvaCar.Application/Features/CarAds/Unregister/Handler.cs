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
    public class UnregisterCarAdByIdCommandHandler : IRequestHandler<UnregisterCarAdByIdCommand>
    {
        private readonly ICarAdRepository _carAdRepository;
        private readonly IClockService _clockService;
        private readonly ICurrentUserService _currentUserService;

        public UnregisterCarAdByIdCommandHandler
            (
                ICarAdRepository carAdRepository,
                IClockService clockService,
                ICurrentUserService currentUserService
            )
        {
            _carAdRepository = carAdRepository;
            _clockService = clockService;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UnregisterCarAdByIdCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetCurrentUser().Id;

            var carAd = await _carAdRepository.GetByUserAndAdIdsAsync(userId, command.Id, cancellationToken);
            Check.IsNotNull<EntityNotFoundException>(carAd, $"Entidad con Id {command.Id} no encontrada.");

            carAd.Unregister(_clockService.Now);
            await _carAdRepository.UpdateAsync(carAd,cancellationToken);

            return Unit.Value;
        }
    }
}
