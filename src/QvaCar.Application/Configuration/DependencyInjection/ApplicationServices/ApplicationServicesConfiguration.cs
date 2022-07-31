using Microsoft.Extensions.DependencyInjection;
using QvaCar.Seedwork.Domain.Services;

namespace QvaCar.Application.Configuration
{
    internal static class ApplicationServicesConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IClockService, ClockService>();
            return services;
        }
    }
}