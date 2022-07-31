using MediatR;
using QvaCar.Application.Common.Secutity;

namespace QvaCar.Application.Features.CarAds
{
    [AnySubscribedUser]
    public record GetCarAdsByUserCommand : IRequest<GetCarAdsByUserResponse> { }
}
