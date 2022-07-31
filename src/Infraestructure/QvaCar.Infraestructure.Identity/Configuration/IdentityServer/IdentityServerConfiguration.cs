using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Infraestructure.Identity.DbContext;
using QvaCar.Infraestructure.Identity.Models;

namespace QvaCar.Infraestructure.Identity.Configuration
{
    public static class IdentityServerConfiguration
    {
        public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration, string migrationsAssembly, IdentityOptions identityOptions)
        {
            var builder = services.AddIdentityServer(options =>
            {
                options.IssuerUri = identityOptions.Issuer;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<QvaCarIdentityUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(identityOptions.DatabaseConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                options.DefaultSchema = EntityConfigurationConstants.IdentityConfTablesSchema;
            })
            .AddOperationalStore(options =>
           {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(identityOptions.DatabaseConnectionString,
                       sql => sql.MigrationsAssembly(migrationsAssembly));
               options.EnableTokenCleanup = true;
               options.TokenCleanupInterval = 30;
               options.DefaultSchema = EntityConfigurationConstants.GrantsTablesSchema;
           });

            builder.AddDeveloperSigningCredential();
            return services;
        }
    }
}