using QvaCar.Api.FunctionalTests.SeedWork;
using System;

namespace QvaCar.Api.FunctionalTests.Shared.Mocks.Extensions
{
    public static class MocksFixtureExtensions
    {
        public static void AssumeClockNowAt(this TestServerFixture _, DateTime now)
        {
            MockClockService.AssumeNowAs(now);
        }

        public static void AssumeClockUtcNowAt(this TestServerFixture _, DateTime utcNow)
        {
            MockClockService.AssumeUtcNowAs(utcNow);
        }
    }
}
