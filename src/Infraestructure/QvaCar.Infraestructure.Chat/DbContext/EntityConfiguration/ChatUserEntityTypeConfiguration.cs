using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QvaCar.Domain.Chat;

namespace QvaCar.Infraestructure.Chat
{
    public class ChatUserEntityTypeConfiguration : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder
                .ToTable(EntityConfigurationConstants.Users, EntityConfigurationConstants.ChatTableSchema);
            builder
                .HasKey(x => x.Id);
            builder
                .Property(p => p.Id)
                .ValueGeneratedNever();
            builder
                .HasMany(e => e.Channels)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
