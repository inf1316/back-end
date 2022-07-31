using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Api.Configuration.DependencyInjection;
using QvaCar.Infraestructure.Identity.Configuration;
using QvaCar.Web.Configuration;

namespace QvaCar.Web
{

    public class Startup
    {
        private IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddQvaCarApi(Configuration, Env)
                .AddQvaCarIdentityInfraestructure(Configuration)
                .ConfigureQvaCarWebAndApiMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
               .InititalizateIdentityDatabaseOnStart(env)
               .UseStaticFiles()
               .UseQvaCarApi(env)
               .UseQvaCarWebAndApiMvc();
        }
    }
}
