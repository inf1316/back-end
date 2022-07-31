using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QvaCar.Infraestructure.Identity.Models;
using System;

namespace QvaCar.Infraestructure.Identity.DbContext
{
    public class QvaCarUsersDBContext :
          IdentityDbContext
          <
              QvaCarIdentityUser,
              QvaCarIdentityRole,
              Guid,
              IdentityUserClaim<Guid>,
              QvaCarIdentityUserRole,
              IdentityUserLogin<Guid>,
              IdentityRoleClaim<Guid>,
              IdentityUserToken<Guid>
          >
    {

        public DbSet<QvaCarIdentityUserSubscriptionLevel> SubscriptionLevels;

        public QvaCarUsersDBContext() { }
        public QvaCarUsersDBContext(DbContextOptions<QvaCarUsersDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QvaCarIdentityRole>(b =>
            {
                b.ToTable("Roles", EntityConfigurationConstants.UsersTablesSchema);
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
            {
                b.ToTable("Claims", EntityConfigurationConstants.UsersTablesSchema);
            });

            modelBuilder.Entity<QvaCarIdentityUserRole>(b =>
            {
                b.ToTable("User_Roles", EntityConfigurationConstants.UsersTablesSchema);
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
            {
                b.ToTable("UserLogins", EntityConfigurationConstants.UsersTablesSchema);
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
            {
                b.ToTable("UserTokens", EntityConfigurationConstants.UsersTablesSchema);
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
            {
                b.ToTable("Role_Claims", EntityConfigurationConstants.UsersTablesSchema);
            });

            modelBuilder.ApplyConfiguration(new QvaCarIdentityUserSubscriptionLevelTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QvaCarIdentityUserTypeConfiguration());
        }
    }
}
