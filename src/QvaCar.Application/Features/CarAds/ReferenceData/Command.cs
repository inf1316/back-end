using MediatR;

namespace QvaCar.Application.Features.CarAds
{
    public record GetCarAdsReferenceDataCommand : IRequest<GetCarAdsReferenceDataResponse> { }
}
