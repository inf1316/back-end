using AutoMapper;
using MediatR;
using QvaCar.Application.Exceptions;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Services;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.CarAds
{
    public class CarAdByIdCommandHandler : IRequestHandler<GetCarAdByIdCommand, GetCarAdByIdResponse>
    {
        private readonly ICarAdsQueries _carAdsQueries;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ICurrentUserService _currentUserService;

        public CarAdByIdCommandHandler
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

        public async Task<GetCarAdByIdResponse> Handle(GetCarAdByIdCommand command, CancellationToken cancellationToken)
        {
            var user = _currentUserService.GetCurrentUser();
            var userId = user.Id;           

            var rawAd = await _carAdsQueries.GetCarAdByUserAndAdIdAsync(userId, command.Id, cancellationToken);
            Check.IsNotNull<EntityNotFoundException>(rawAd, $"Entity with Id {command.Id} not found.");

            var response = _mapper.Map<GetCarAdByIdResponse>(rawAd);
            var images = new List<GetCarAdImagebByIdResponse>();

            foreach (var image in rawAd.Images)
            {
                List<ImageByIdResponse> imageVersions = GetImageVersions(userId, command.Id, image);
                images.Add(new GetCarAdImagebByIdResponse() { FileName = image, Urls = imageVersions });
            }
            return response with { Images = images, };
        }

        private List<ImageByIdResponse> GetImageVersions(Guid userId, Guid adId, string image)
        {
            var verisons = _imageService.GetUrlsForImage(userId, adId, image);
            return verisons
                .Select(r => new ImageByIdResponse()
                {
                    Size = r.Type.Name.ToLower(),
                    Url = r.Url
                })
                .ToList();
        }
    }
}
