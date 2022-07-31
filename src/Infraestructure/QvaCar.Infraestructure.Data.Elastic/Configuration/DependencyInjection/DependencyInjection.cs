using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Domain.Search;
using QvaCar.Infraestructure.Data.Elastic.Queries;
using QvaCar.Infraestructure.Data.Elastic.Repositores;
using System.Reflection;

namespace QvaCar.Infraestructure.Data.Elastic.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQvaDataElasticInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            services
                .AddAutoMapper()
                .AddCustomElasticSearch(configuration)
                .AddRepositories()
                .AddQueries();
            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICarAdSearchModelRepository, CarAdSearchModelRepository>();
            return services;
        }

        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddTransient<ISearchCarAdsQueries, SearchCarAdsQueries>();
            return services;
        }
    }
}