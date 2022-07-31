using MediatR;
using QvaCar.Application.Common.Secutity;
using System;

namespace QvaCar.Application.Features.CarAds
{
    [AnySubscribedUser]
    public record PublishCarAdByIdCommand : IRequest 
    {
        public Guid Id { get; init; }
    }
}
