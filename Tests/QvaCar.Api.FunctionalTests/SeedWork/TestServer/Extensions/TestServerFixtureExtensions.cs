using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace QvaCar.Api.FunctionalTests.SeedWork
{
    public static class TestServerFixtureExtensions
    {
        public static async Task ExecuteScopeAsync(this TestServerFixture fixture, Func<IServiceProvider, Task> function)
        {
            var scopeFactory = fixture.Server.Services.GetService<IServiceScopeFactory>();

            if (scopeFactory is null)
                throw new NullReferenceException("Cannot create scope");

            using (var scope = scopeFactory.CreateScope())
            {
                await function(scope.ServiceProvider);
            }
        }

        public static void ExecuteScope(this TestServerFixture fixture, Action<IServiceProvider> action)
        {
            var scopeFactory = fixture.Server.Services.GetService<IServiceScopeFactory>();

            if (scopeFactory is null)
                throw new NullReferenceException("Cannot create scope");

            using (var scope = scopeFactory.CreateScope())
            {
                action(scope.ServiceProvider);
            }
        }
    }
}
