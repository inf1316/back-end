using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds.Services;
using QvaCar.Infraestructure.BlogStorage.Configuration.Options;
using QvaCar.Infraestructure.BlogStorage.Services;

namespace QvaCar.Infraestructure.BlogStorage.Configuration.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQvaCarDataInfraestructureBlogStorage(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddOptions(configuration)
                .AddServices();
        }

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            // See more : https://referbruv.com/blog/posts/working-with-options-pattern-in-aspnet-core-the-complete-guide           
            var imageService = configuration.GetSection(ImageServiceOptions.Section)
                .Get<ImageServiceOptions>();
            services.AddSingleton(imageService);

            var blobServiceClient = new BlobServiceClient(imageService.BlobStorageConnectionString);
            var container = blobServiceClient.GetBlobContainerClient(imageService.ContainerName);
            services.AddSingleton(s => container);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFileSystemService, FileSystemService>();
            return services;
        }

        public static IApplicationBuilder InitializeBlobStorage(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (!env.IsDevelopment())
                return app;

            CreateConatainerIfDontExist(app, env);
            return app;
        }

        private static void CreateConatainerIfDontExist(IApplicationBuilder app, IHostEnvironment env)
        {
            var serviceFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            if (serviceFactory is null)
                throw new System.Exception("Service Factory is not ready");

            using (var serviceScope = serviceFactory.CreateScope())
            {
                var blobClient = serviceScope.ServiceProvider.GetRequiredService<BlobContainerClient>();
                blobClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }
        }
    }
}
