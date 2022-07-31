using System.Diagnostics.CodeAnalysis;
using Xunit;
namespace QvaCar.Api.FunctionalTests.SeedWork
{
    [ExcludeFromCodeCoverage]
    [CollectionDefinition(nameof(TestServerFixtureCollection))]
    public class TestServerFixtureCollection : ICollectionFixture<TestServerFixture> { }
}
