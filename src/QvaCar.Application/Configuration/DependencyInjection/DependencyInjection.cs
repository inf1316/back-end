using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QvaCar.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQvaCarApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddApplicationServices()
                .AddCustomAutomapper()
                .AddCustomFluentValidation()
                .AddCustomMediatR();
            
            return services;
        }
    }
}