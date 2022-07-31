using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace QvaCar.Web.Configuration
{
    internal static class MvcConfiguration
    {
        public static IServiceCollection ConfigureQvaCarWebAndApiMvc(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            return services;
        }

        public static IApplicationBuilder UseQvaCarWebAndApiMvc(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            return app;
        }
    }   
}