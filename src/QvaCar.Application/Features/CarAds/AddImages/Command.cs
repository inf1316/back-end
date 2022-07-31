using MediatR;
using QvaCar.Application.Common.Secutity;
using System;

namespace QvaCar.Application.Features.CarAds
{
    [AnySubscribedUser]
    public record AddImagesToCarAdCommand : IRequest
    {
        public Guid Id { get; init; }        
        public ImageStream[] Images { get; init; } = Array.Empty<ImageStream>();
    }

    public class ImageStream
    {
        public string FileName { get; init; } = string.Empty;
        public byte[] File { get; init; } = Array.Empty<byte>();
        public string ContentType { get; init; } = string.Empty;
    }
}
