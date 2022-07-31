using QvaCar.Domain.Identity;
using System;

namespace QvaCar.Api.FunctionalTests.Shared.Identity
{
    public class TestApiUserBuilder
    {
        private Guid _id;
        private UserSubscriptionLevel _subscriptionLevel = null;

        public TestApiUserBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public TestApiUserBuilder WithSubscriptionLevel(UserSubscriptionLevel subscriptionLevel)
        {
            _subscriptionLevel = subscriptionLevel;
            return this;
        }
        public TestApiUserBuilder WithFreeSubscriptionLevel()
        {
            _subscriptionLevel = UserSubscriptionLevel.Free;
            return this;
        }

        public TestApiUserBuilder WithPaidSubscriptionLevel()
        {
            _subscriptionLevel = UserSubscriptionLevel.Free;
            return this;
        }

        public TestApiUser Build() => new TestApiUser(_id, _subscriptionLevel);
    }
}