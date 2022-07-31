using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using QvaCar.Infraestructure.Data.Elastic.Configuration.Options;
using System;

namespace QvaCar.Infraestructure.Data.Elastic.Configuration
{
    public static class ElasticSearchConfigurationExtensions
    {
        public static IServiceCollection AddCustomElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            BindElasticSearchOptions(services, configuration);

            services.AddSingleton<IElasticClient, ElasticClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<ElasticSearchConfiguration>>().Value;
                var pool = new SingleNodeConnectionPool(new Uri(options.ServerEndpoint));
                var settings = new ConnectionSettings(pool) 
                    .EnableDebugMode()
                    .DefaultIndex(options.CarAdsIndexName)
                    .AddQvaCarDefaultMappings();
                var elasticClient = new ElasticClient(settings);
                return elasticClient;
            });

            services.AddSingleton<QvaCarIndexContext>();
            return services;
        }

        public static IApplicationBuilder InitializeElasticSearchDB(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            if (serviceFactory is null)
                throw new System.Exception("Service Factory is not ready");

            using (var serviceScope = serviceFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<QvaCarIndexContext>();
                context.CreateIndexIfNotExistAsync().GetAwaiter().GetResult();
            }
            return app;
        }

        private static void BindElasticSearchOptions(IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchOptions = new ElasticSearchConfiguration();
            configuration.GetSection(ElasticSearchConfiguration.SectionName).Bind(elasticSearchOptions);

            services.Configure<ElasticSearchConfiguration>(configuration.GetSection(ElasticSearchConfiguration.SectionName));
        }

        private static ConnectionSettings AddQvaCarDefaultMappings(this ConnectionSettings connectionSettings)
        {
            QvaCarIndexContext.AddQvaCarDefaultMappings(connectionSettings);
            return connectionSettings;
        }
    }  
}
