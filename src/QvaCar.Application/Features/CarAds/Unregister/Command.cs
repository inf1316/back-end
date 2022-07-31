using MediatR;
using QvaCar.Application.Common.Secutity;
using System;

namespace QvaCar.Application.Features.CarAds
{
    [AnySubscribedUser]
    public record UnregisterCarAdByIdCommand : IRequest 
    {
        public Guid Id { get; init; }
    }
}
