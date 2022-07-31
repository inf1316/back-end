using QvaCar.Seedwork.Domain;
using System;

namespace QvaCar.Domain.Identity
{
    public record UserRegisteredDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int ProvinceId { get; set; }
        public UserSubscriptionLevel SubscriptionLevel { get; set; }

        public UserRegisteredDomainEvent
            (
                Guid id,
                string email,
                string firstName,
                string lastName,
                int age,
                string address,
                int provinceId,
                UserSubscriptionLevel subscriptionLevel
            )
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Address = address;
            ProvinceId = provinceId;
            SubscriptionLevel = subscriptionLevel;
        }

        public static UserRegisteredDomainEvent FromUser(QvaCarUser user)
        {
            return new UserRegisteredDomainEvent(user.Id, user.Email, user.FirstName, user.LastName, user.Age, user.Address, user.ProvinceId, user.SubscriptionLevel);
        }
    }
}
