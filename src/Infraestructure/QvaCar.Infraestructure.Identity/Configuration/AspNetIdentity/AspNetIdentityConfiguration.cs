using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Infraestructure.Identity.DbContext;
using QvaCar.Infraestructure.Identity.Models;
using System;

namespace QvaCar.Infraestructure.Identity.Configuration
{
    public static class AspNetIdentityConfiguration
    {
        public static IServiceCollection AddCustomAspNetCoreIdentity(this IServiceCollection services, IdentityOptions identityOptions, string migrationsAssembly)
        {
            services.AddDbContext<QvaCarUsersDBContext>(o =>
            {
                string userCS = identityOptions.DatabaseConnectionString;
                o.UseSqlServer(userCS, sql => sql.MigrationsAssembly(migrationsAssembly));
            });

            services
                .AddIdentity<QvaCarIdentityUser, QvaCarIdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = true;

                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<QvaCarUsersDBContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));
            return services;
        }
    }
}