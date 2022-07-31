using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Application.Services;
using QvaCar.Infraestructure.Identity.Services;
using System.Reflection;

namespace QvaCar.Infraestructure.Identity.Configuration
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddQvaCarIdentityInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var identityOptions = new IdentityOptions();
            configuration.GetSection(IdentityOptions.SectionName).Bind(identityOptions);
            services.Configure<IdentityOptions>(configuration.GetSection(IdentityOptions.SectionName));
            var migrationsAssemblyName = typeof(DependencyInjectionExtensions).GetTypeInfo().Assembly.GetName().Name ?? string.Empty;

            services
                   .AddOptions(configuration)
                   .AddServices()
                   .AddCustomAspNetCoreIdentity(identityOptions, migrationsAssemblyName)
                   .AddIdentityServer(configuration, migrationsAssemblyName, identityOptions);

            return services;
        }

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Section));
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUserAuthService, UserAuthService>();
            services.AddTransient<IQvaCarAuthorizationService, QvaCarAuthorizationService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            return services;
        }

        public static IApplicationBuilder InititalizateIdentityDatabaseOnStart(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            IndentityServerDatabaseSeeder.InitializeDatabase(app,env);
            IdentityUsersSeeder.CreateIdentitySeedData(app, env);
            return app;
        }
    }
}