using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Application.Configuration;
using QvaCar.Infraestructure.BlogStorage.Configuration.DependencyInjection;
using QvaCar.Infraestructure.Configuration;

namespace QvaCar.Api.Configuration.DependencyInjection
{
    public static class ApiDependencyInjectionExtensions
    {
        public static IServiceCollection AddQvaCarApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services
                .ConfigureAutoMapper()
                .AddQvaCarApplication(configuration)
                .ConfigureApiVersion()
                .AddApplicationInsights(configuration)
                .AddQvaCarDataInfraestructure(configuration)
                .AddQvaCarDataInfraestructureBlogStorage(configuration)
                .AddQvaCarApiAuth(configuration, env)
                .ConfigureCustomApiBehavior()
                .ConfigureProblemDetails()
                .ConfigureCustomSwagger(configuration, env);
            return services;
        }

        public static IApplicationBuilder UseQvaCarApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .InitializeBlobStorage(env)
                .UseCustomProblemDetails()
                .UseCustomSwagger(env);
            return app;
        }
    }
}
