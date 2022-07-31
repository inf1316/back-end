using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.Identity;
using QvaCar.Infraestructure.Identity.DbContext;
using QvaCar.Infraestructure.Identity.Models;
using System;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Identity.Configuration
{
    public class IdentityUsersSeeder
    {
        public static void CreateIdentitySeedData(IApplicationBuilder app, IHostEnvironment env)
        {
            var serviceFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            if (serviceFactory is null)
                throw new Exception("Service Factory is not ready");

            using (var serviceScope = serviceFactory.CreateScope())
            {
                MigrateDatabaseIfNeededAsync(serviceScope,env).GetAwaiter().GetResult();
                CreateRoles(serviceScope).GetAwaiter().GetResult();
                CreateAdminAccountsAsync(serviceScope).GetAwaiter().GetResult();
            }

        }

        private static async Task MigrateDatabaseIfNeededAsync(IServiceScope serviceScope, IHostEnvironment env)
        {
            if (!env.ShouldApplyMigrations())
                return;

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<QvaCarUsersDBContext>();
            await dbContext.Database.MigrateAsync();
        }

        private static async Task CreateRoles(IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<QvaCarIdentityRole>>();

            var roles = new[] { QvaCarIdentityRole.RegularUserRole, QvaCarIdentityRole.AdminRole };
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role.Name) == null)
                    await roleManager.CreateAsync(role);
            }
        }

        private static async Task CreateAdminAccountsAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<QvaCarIdentityUser>>();
            var accountService = serviceScope.ServiceProvider.GetRequiredService<IUserAccountService>();

            UserRegistrationServiceRequest admin1Request = BuildAdminUser1Request();
            UserRegistrationServiceRequest admin2Request = BuildAdminUser2Request();

            await CreateUserIfNotExists(userManager, accountService, admin1Request);
            await CreateUserIfNotExists(userManager, accountService, admin2Request);
        }

        private static UserRegistrationServiceRequest BuildAdminUser1Request()
        {
            var id = Guid.Parse("41a7c14c-a68c-4206-977b-bd8bc0d2ffb4");
            var email = "mclanghlin@gmail.com";
            var password = "Maki@2021";
            var firstName = "José Manuel";
            var lastName = "Mc Langhin";
            var age = 27;
            var address = "Selva 27";
            var provinceId = Province.Cienfuegos.Id;
            var subscriptionLevel = UserSubscriptionLevel.Paid;

            var request = new UserRegistrationServiceRequest()
            {
                User = QvaCarUser.CreateExisting(id, email, firstName, lastName, age, address, provinceId, subscriptionLevel),
                Password = password,
                SkipEmailConfirmation = true,
                CreateAsAdmin = true,
            };

            return request;
        }

        private static UserRegistrationServiceRequest BuildAdminUser2Request()
        {
            var id = Guid.Parse("129434e3-2647-48bf-93d3-89da83478844");
            var email = "josecdom94@gmail.com";
            var password = "Blablacar@1";
            var firstName = "Jose Carlos";
            var lastName = "Nuñez";
            var age = 26;
            var address = "Yugoslavia 7";
            var provinceId = Province.Cienfuegos.Id;
            var subscriptionLevel = UserSubscriptionLevel.Paid;

            var request = new UserRegistrationServiceRequest()
            {
                User = QvaCarUser.CreateExisting(id, email, firstName, lastName, age, address, provinceId, subscriptionLevel),
                Password = password,
                SkipEmailConfirmation = true,
                CreateAsAdmin = true,
            };
            return request;
        }

        private static async Task CreateUserIfNotExists(
            UserManager<QvaCarIdentityUser> userManager,
            IUserAccountService accountService,
            UserRegistrationServiceRequest request)
        {
            if (await userManager.FindByNameAsync(request.User.Email) == null)
            {
                await accountService.RegisterUserAsync(request, default);
            }
        }
    }
}