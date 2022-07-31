using AutoMapper;
using MediatR;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.CarAds
{
    public class GetCarAdsByUserCommandHandler : IRequestHandler<GetCarAdsByUserCommand, GetCarAdsByUserResponse>
    {
        private readonly ICarAdsQueries _carAdsQueries;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ICurrentUserService _currentUserService;

        public GetCarAdsByUserCommandHandler
            (
                ICarAdsQueries carAdsQueries,
                IMapper mapper,
                IImageService imageService,
                ICurrentUserService currentUserService
            )
        {
            _carAdsQueries = carAdsQueries;
            _mapper = mapper;
            _imageService = imageService;
            _currentUserService = currentUserService;
        }

        public async Task<GetCarAdsByUserResponse> Handle(GetCarAdsByUserCommand command, CancellationToken cancellationToken)
        {
            var user = _currentUserService.GetCurrentUser();
            var userId = user.Id;

            var rawAds = await _carAdsQueries.GetCarAdsByUserAsync(userId, cancellationToken);
            var responseAds = new List<CarAdByUserItemResponse>();
            foreach (var car in rawAds)
            {
                var elementAssingImage = _mapper.Map<CarAdByUserItemResponse>(car);
                var images = new List<GetCarAdsImageByUserResponse>();

                foreach (var image in car.Images)
                {
                    List<ImageByUserResponse> imageVersions = GetImagesVersions(userId, car.Id, image);
                    images.Add(new GetCarAdsImageByUserResponse() { FileName = image, Urls = imageVersions });
                }
                responseAds.Add(elementAssingImage with { Images = images });
            }
            return new GetCarAdsByUserResponse() { Ads = responseAds };
        }

        private List<ImageByUserResponse> GetImagesVersions(Guid userId, Guid adId, string image)
        {
            var versions = _imageService.GetUrlsForImage(userId, adId, image);
            return versions
                .Select(r => new ImageByUserResponse()
                {
                    Size = r.Type.Name.ToLower(),
                    Url = r.Url
                })
                .ToList();
        }
    }
}
