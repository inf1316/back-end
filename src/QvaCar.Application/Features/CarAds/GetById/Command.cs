using MediatR;
using QvaCar.Application.Common.Secutity;
using System;

namespace QvaCar.Application.Features.CarAds
{
    [AnySubscribedUser]
    public record GetCarAdByIdCommand : IRequest<GetCarAdByIdResponse> 
    {
        public Guid Id { get; init; }
    }
}
