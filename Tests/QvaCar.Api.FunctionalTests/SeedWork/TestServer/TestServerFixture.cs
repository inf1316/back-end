using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QvaCar.Infraestructure.BlogStorage.Configuration.Options;
using QvaCar.Infraestructure.Data;
using QvaCar.Seedwork.Domain.Services;
using QvaCar.Web;
using System;
using System.Threading.Tasks;
using WebMotions.Fake.Authentication.JwtBearer;

namespace QvaCar.Api.FunctionalTests.SeedWork
{
    public sealed class TestServerFixture : IDisposable
    {
        public TestServer Server { get; }
        private static TestServerFixture? FixtureInstance { get; set; }
        public TestServerFixture()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json");
                })
                .UseEnvironment("Test")
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>();
                    webHost.ConfigureTestServices(services =>
                    {
                        System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                        services.SwapSingleton<IClockService, MockClockService>(MockClockService.Service);
                        var apiScheme = JwtBearerDefaults.AuthenticationScheme;
                        services.AddAuthentication(apiScheme).AddFakeJwtBearer(apiScheme, options =>
                        {
                            options.BearerValueType = FakeJwtBearerBearerValueType.Base64;
                        });
                    });
                });


            var host = hostBuilder.StartAsync().GetAwaiter().GetResult();
            this.Server = host.GetTestServer();
            FixtureInstance = this;

            RecreateDatabasesOnInit().GetAwaiter().GetResult();
            RecreateBlobStorageOnStart();
        }
        public void Dispose()
        {
            Server.Dispose();
        }

        #region Recreate Databases On Start
        private static async Task RecreateDatabasesOnInit()
        {
            await RecreateCosmosDbBeforeStart();
        }
        private static async Task RecreateCosmosDbBeforeStart()
        {
            if (FixtureInstance is null)
                throw new Exception("Test Setup Error");

            await FixtureInstance.ExecuteScopeAsync(async services =>
            {
                var dbContext = services.GetService<QvaCarDbContext>();

                if (dbContext is null)
                    throw new ArgumentNullException($"{nameof(QvaCarDbContext)} is not registered in the IoC container.");

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();
            });
        }
        #endregion

        #region Recreate Blob Storage on Start
        private static void RecreateBlobStorageOnStart()
        {
            if (FixtureInstance is null)
                throw new Exception("Test Setup Error");

            FixtureInstance.ExecuteScope(services =>
            {
                var imageService = services.GetRequiredService<ImageServiceOptions>();
                var blobServiceClient = new BlobServiceClient(imageService.BlobStorageConnectionString);
                var container = blobServiceClient.GetBlobContainerClient(imageService.ContainerName);
                container.CreateIfNotExists(PublicAccessType.BlobContainer);
                var dbContext = services.GetService<QvaCarDbContext>();
            });
        }
        #endregion


        #region Reset Application State Between Test
        public static void OnTestInitResetApplicationState()
        {
            OnTestInitResetCosmosBd();
            OnTestInitResetApplicationServices();
            OnTestInitResetBlogStorage();
        }

        private static void OnTestInitResetCosmosBd()
        {
            FixtureInstance?.ExecuteScope(services =>
            {
                var dbContext = services.GetService<QvaCarDbContext>();

                if (dbContext is null)
                    throw new ArgumentNullException($"{nameof(QvaCarDbContext)} is not registered in the IoC container.");

                dbContext.ClearDatabaseBeforeTestAsync().GetAwaiter().GetResult();
            });

        }

        private static void OnTestInitResetBlogStorage()
        {
            var blobContainerClient = FixtureInstance?.Server.Services.GetService<BlobContainerClient>();
            if (blobContainerClient is null)
                throw new ArgumentNullException($"{nameof(BlobContainerClient)} is not registered in the IoC container.");

            var imageOption = FixtureInstance?.Server.Services.GetService<ImageServiceOptions>();
            if (imageOption is null)
                throw new ArgumentNullException($"{nameof(ImageServiceOptions)} is not registered in the IoC container.");

            var blobsToBeDeleted = blobContainerClient.GetBlobs();
            foreach (var blob in blobsToBeDeleted)
                blobContainerClient.DeleteBlobIfExists(blob.Name);
        }

        private static void OnTestInitResetApplicationServices()
        {
            MockClockService.ResetService();
        }
        #endregion
    }
}
