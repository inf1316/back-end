using QvaCar.Seedwork.Domain;
using System;
using System.Linq;

namespace QvaCar.Domain.CarAds
{
    public record ImageAddedToAdDomainEvent : IDomainEvent
    {
        public Guid AdId { get; init; }
        public int StateId { get; init; }
        public ImageDto[] Images { get; init; }

        private ImageAddedToAdDomainEvent(Guid adId, int adCurrentStateId, ImageDto[] images)
        {
            AdId = adId;
            StateId = adCurrentStateId;
            Images = images;
        }

        public static ImageAddedToAdDomainEvent FromImages(Guid AdId, int adCurrentStateId, string[] addedImagesFileNamesWithExtension)
        {
            return new ImageAddedToAdDomainEvent(AdId, adCurrentStateId, addedImagesFileNamesWithExtension.Select(x => new ImageDto(x)).ToArray());
        }

        public record ImageDto
        {
            public string FileNamesWithExtension { get; init; }

            public ImageDto(string fileNamesWithExtension)
            {
                FileNamesWithExtension = fileNamesWithExtension;
            }
        }
    }
}
