using IdGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QvaCar.Domain.Chat;
using QvaCar.Domain.Chat.Services;
using QvaCar.Infraestructure.Chat.Queries;
using QvaCar.Infraestructure.Chat.Repositories;
using QvaCar.Infraestructure.Chat.Services;
using QvaCar.Infraestructure.Data.DbContextQuery;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Chat.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQvaCarChatInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddChatDbContext(configuration)
                .AddRepositories()
                .AddQueries()
                .AddServices();

            return services;
        }

        private static IServiceCollection AddChatDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlOptions = configuration.GetSection(ChatDbSettings.SectionName).Get<ChatDbSettings>();
            services.AddDbContext<QvaCarChatDbContext>(options =>
            {
                string userCS = sqlOptions.DatabaseConnectionString;
                options.UseSqlServer(userCS);
            })
             .AddTransient<IQvaCarChatQuery, QvaCarChatQuery>(opts => new QvaCarChatQuery(new SqlConnection(sqlOptions.DatabaseConnectionString)));
            return services;
        }

        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddTransient<IChatQueries, ChatQueries>();
            services.AddTransient<IChatCarAdQueries, ChatCarAdQueries>();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            var generatorId = 182;
            services.AddSingleton<IdGenerator>(_ => new IdGenerator(generatorId));
            services.AddSingleton<IMessageSecuenceIdGenerator, MessageSecuenceIdGenerator>();
            services.AddTransient<IChatDomainService, ChatDomainService>();
            services.AddTransient<IChatReadOnlyImageService, ChatReadOnlyImageService>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IChatUserRepository, ChatUserRepository>();
            return services;
        }

        public static IApplicationBuilder InitializeChatDatabaseForDevelopment(this IApplicationBuilder app, IHostEnvironment env)
        {
            var serviceFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            if (serviceFactory is null)
                throw new System.Exception("Service Factory is not ready");

            using (var serviceScope = serviceFactory.CreateScope())
            {
                MigrateChatDatabaseForDevelopment(serviceScope, env).GetAwaiter().GetResult();
            }
            return app;
        }

        private static async Task MigrateChatDatabaseForDevelopment(IServiceScope serviceScope, IHostEnvironment env)
        {
            if (!env.ShouldApplyMigrations())
                return;
            await serviceScope.ServiceProvider.GetRequiredService<QvaCarChatDbContext>().Database.MigrateAsync();
        }

        public static bool ShouldApplyMigrations(this IHostEnvironment env) => env.IsDevelopment() || env.IsTesting();
        public static bool IsTesting(this IHostEnvironment env) => env.EnvironmentName == "Test";
    }
}
