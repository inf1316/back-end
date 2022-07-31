using System;

namespace QvaCar.Api.Configuration
{
    public class ApplicationInsightsOptions
    {
        public const string SectionName = "ApplicationInsights";
        public string InstrumentationKey { get; init; } = String.Empty;
    }
}
