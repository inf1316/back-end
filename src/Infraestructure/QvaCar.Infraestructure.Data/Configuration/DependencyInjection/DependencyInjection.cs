using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using QvaCar.Infraestructure.Data;
using QvaCar.Infraestructure.Data.Queries;
using QvaCar.Infraestructure.Data.Repositories;

namespace QvaCar.Infraestructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQvaCarDataInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddQvaCarDbContext(configuration)
                .AddQueries()
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddQvaCarDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosOptions = configuration.GetSection(CosmosDbOptions.Section).Get<CosmosDbOptions>();
            services.AddDbContext<QvaCarDbContext>(options => options.UseCosmos(cosmosOptions.AccountEndpoint, cosmosOptions.AccountKey, cosmosOptions.DatabaseName));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAdStateRepository, AdStateRepository>();
            services.AddTransient<ICarBodyTypeRepository, CarBodyTypeRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<IFuelTypeRepository, FuelTypeRepository>();
            services.AddTransient<IGearboxTypeRepository, GearboxTypeRepository>();
            services.AddTransient<IProvinceRepository, ProvinceRepository>();
            services.AddTransient<ICarAdRepository, CarAdRepository>();
            services.AddTransient<IExteriorTypeRepository, ExteriorTypeRepository>();
            services.AddTransient<IInsidesTypesRepository, InsidesTypesRepository>();
            services.AddTransient<ISafetyTypeRepository, SafetyTypeRepository>();

            return services;
        }

        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddTransient<ICarAdsQueries, CarAdsQueries>();
            return services;
        }
    }
}