using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace QvaCar.Api.Configuration
{
    internal static class CustomApiBehaviourConfiguration
    {
        public static IServiceCollection ConfigureCustomApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ProblemDetailsConfiguration.ProblemDetailsApiBehaviorConfiguration;
            });
            return services;
        }
    }
}