using Microsoft.Extensions.Hosting;

namespace QvaCar.Infraestructure.Identity.Configuration
{
    public static class EnvironmentExtensions
    {
        public static bool IsTesting(this IHostEnvironment env) => env.EnvironmentName == "Test";
        public static bool ShouldApplyMigrations(this IHostEnvironment env) => env.IsDevelopment() || env.IsTesting();
    }
}
