using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Identity.Configuration
{
    public static class IndentityServerDatabaseSeeder
    {
        public static void InitializeDatabase(IApplicationBuilder app, IHostEnvironment env)
        {
            var serviceFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            if (serviceFactory is null)
                throw new System.Exception("Service Factory is not ready");

            using (var serviceScope = serviceFactory.CreateScope())
            {
                MigratePersistenceGrantDbAsync(serviceScope, env).GetAwaiter().GetResult();
                MigrateAndConfigureIdentityServerConfigurationDbAsync(serviceScope, env).GetAwaiter().GetResult();
            }
        }

        #region Grants Db
        private static async Task MigratePersistenceGrantDbAsync(IServiceScope serviceScope, IHostEnvironment env)
        {
            if (!env.ShouldApplyMigrations())
                return;
            await serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();
        }
        #endregion

        #region Configuration DB

        private static async Task MigrateAndConfigureIdentityServerConfigurationDbAsync(IServiceScope serviceScope, IHostEnvironment env)
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            var identityOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>();

            await MigrateConfigurationDatabaseIfNeeded(context, env);

            await AddIdentityResourcesAsync(context);
            await AddClientsAsync(env, context, identityOptions);
            await AddApiScopesAsync(context);
            await AddResourcesAsync(context);
        }

        private static async Task MigrateConfigurationDatabaseIfNeeded(ConfigurationDbContext context, IHostEnvironment env)
        {
            if (!env.ShouldApplyMigrations())
                return;
            await context.Database.MigrateAsync();
        }

        private static async Task AddIdentityResourcesAsync(ConfigurationDbContext context)
        {
            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerClientsConfiguration.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }
        }

        private static async Task AddClientsAsync(IHostEnvironment env, ConfigurationDbContext context, IOptions<IdentityOptions> identityOptions)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in IdentityServerClientsConfiguration.GetClients(env, identityOptions.Value))
                {
                    context.Clients.Add(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }
        }

        private static async Task AddApiScopesAsync(ConfigurationDbContext context)
        {
            if (!context.ApiScopes.Any())
            {
                foreach (var resource in IdentityServerClientsConfiguration.GetApiScopes())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }
        }

        private static async Task AddResourcesAsync(ConfigurationDbContext context)
        {
            if (!context.ApiResources.Any())
            {
                foreach (var resource in IdentityServerClientsConfiguration.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }
        }
        #endregion
    }
}