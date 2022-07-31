using IdGen;
using QvaCar.Domain.CarAds.Services;
using QvaCar.Domain.Chat;
using QvaCar.Domain.Chat.Services;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Chat.Services
{
    public class ChatReadOnlyImageService : IChatReadOnlyImageService
    {
        private readonly IImageService _imageService;

        public ChatReadOnlyImageService(IImageService imageService) => _imageService = imageService;

        public List<ChatImageVersionResponse> GetUrlsForImageForCarAds(Guid userId, Guid adId, string fileNameWithExtension)
        {
            var originalResponse = _imageService.GetUrlsForImage(userId, adId, fileNameWithExtension);
            return originalResponse
                .Select(x =>
                {
                    var type = Enumeration.GetAll<ChatImageType>().Single(y => y.Id == x.Type.Id);
                    return new ChatImageVersionResponse(type, x.Url);
                })
                .ToList();
        }
    }
}
