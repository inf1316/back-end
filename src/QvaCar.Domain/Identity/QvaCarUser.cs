using QvaCar.Seedwork.Domain;
using System;

namespace QvaCar.Domain.Identity
{
    public class QvaCarUser : AggregateRoot<Guid>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int ProvinceId { get; set; }
        public UserSubscriptionLevel SubscriptionLevel { get; set; }

        private QvaCarUser(Guid id, string email, string firstName, string lastName, int age, string address, int provinceId,
                         UserSubscriptionLevel subscriptionLevel)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Address = address;
            ProvinceId = provinceId;
            SubscriptionLevel = subscriptionLevel;
            this.AddDomainEvent(UserRegisteredDomainEvent.FromUser(this));
        }

        private QvaCarUser(string email, string firstName, string lastName, int age, string address, int provinceId, UserSubscriptionLevel subscriptionLevel)
                        : this(Guid.NewGuid(), email, firstName, lastName, age, address, provinceId, subscriptionLevel)
        { }

        public static QvaCarUser CreateNew(string email, string firstName, string lastName, int age, string address, int provinceId,
                          UserSubscriptionLevel subscriptionLevel)
        {
            return new QvaCarUser(email, firstName, lastName, age, address, provinceId, subscriptionLevel);
        }

        public static QvaCarUser CreateExisting(Guid id, string email, string firstName, string lastName, int age, string address, int provinceId,
                  UserSubscriptionLevel subscriptionLevel)
        {
            return new QvaCarUser(id, email, firstName, lastName, age, address, provinceId, subscriptionLevel);
        }
    }
}