using QvaCar.Domain.Identity;
using System;

namespace QvaCar.Api.FunctionalTests.Shared.Identity
{
    public class TestApiUser
    {
        public Guid Id { get; private set; }
        public UserSubscriptionLevel SubscriptionLevel { get; private set; } = UserSubscriptionLevel.Free;

        public TestApiUser(Guid id, UserSubscriptionLevel subscriptionLevel)
        {
            Id = id;
            SubscriptionLevel = subscriptionLevel;
        }
    }
}
