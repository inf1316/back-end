using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QvaCar.Infraestructure.Identity.Models;

namespace QvaCar.Infraestructure.Identity.DbContext
{
    internal class QvaCarIdentityUserTypeConfiguration : IEntityTypeConfiguration<QvaCarIdentityUser>
    {
        public void Configure(EntityTypeBuilder<QvaCarIdentityUser> builder)
        {
            builder.ToTable(EntityConfigurationConstants.Users, EntityConfigurationConstants.UsersTablesSchema);
            builder
              .HasOne(e => e.SubscriptionLevel)
              .WithMany()
              .HasForeignKey(m => m.SubscriptionLevelId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
