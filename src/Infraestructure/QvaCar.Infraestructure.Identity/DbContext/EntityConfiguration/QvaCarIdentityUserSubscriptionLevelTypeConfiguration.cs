using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QvaCar.Domain.Identity;
using QvaCar.Infraestructure.Identity.Models;
using QvaCar.Seedwork.Domain;
using System.Linq;

namespace QvaCar.Infraestructure.Identity.DbContext
{
    internal class QvaCarIdentityUserSubscriptionLevelTypeConfiguration : IEntityTypeConfiguration<QvaCarIdentityUserSubscriptionLevel>
    {
        public void Configure(EntityTypeBuilder<QvaCarIdentityUserSubscriptionLevel> builder)
        {
            builder.ToTable(EntityConfigurationConstants.SubscriptionLevels, EntityConfigurationConstants.UsersTablesSchema);

            builder.HasKey(m => m.Id);

            builder
               .Property(m => m.Name)
               .HasMaxLength(50);

            var seedData = Enumeration
                .GetAll<UserSubscriptionLevel>()
                .Select(x => new QvaCarIdentityUserSubscriptionLevel(x.Id, x.Name))
                .ToArray();

            builder.HasData(seedData);
        }
    }
}
