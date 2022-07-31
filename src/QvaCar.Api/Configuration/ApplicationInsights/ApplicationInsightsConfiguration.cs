using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QvaCar.Api.Configuration
{
    public static class ApplicationInsightsConfiguration
    {
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationInsights = new ApplicationInsightsOptions();
            configuration.GetSection(ApplicationInsightsOptions.SectionName).Bind(applicationInsights);
            
            services.AddApplicationInsightsTelemetry(applicationInsights.InstrumentationKey);            
            return services;
        }
    }
}
