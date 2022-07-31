using QvaCar.Seedwork.Domain;
using System;

namespace QvaCar.Domain.CarAds
{
    public record AdUnregisteredDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; init; }
        
        public static AdUnregisteredDomainEvent FromCarAd(CarAd ad)
        {
            return new AdUnregisteredDomainEvent()
            {
                Id = ad.Id,
                UserId = ad.UserId,               
            };
        }
    }
}
