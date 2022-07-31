using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QvaCar.Domain.Chat;

namespace QvaCar.Infraestructure.Chat
{
    public class ChannelEntityTypeConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.ToTable(EntityConfigurationConstants.Channels, EntityConfigurationConstants.ChatTableSchema);
            builder
                .HasKey(x => x.Id);

            builder
                .Property(p => p.Id)
                .ValueGeneratedNever();

            builder.OwnsOne(m => m.AboutAd, propertyBuilder =>
            {
                propertyBuilder.Property(m => m.Id).IsRequired();
                propertyBuilder.Property(m => m.ModelVersion).IsRequired();
                propertyBuilder.Property(m => m.MainImageFileName);

            });

            builder.OwnsOne(m => m.AnotherParticipant, propertyBuilder =>
            {
                propertyBuilder.Property(m => m.Id).IsRequired();
                propertyBuilder.Property(m => m.FirstName).IsRequired();
                propertyBuilder.Property(m => m.LastName).IsRequired();
            });

            builder.OwnsOne(m => m.LastMessage, propertyBuilder =>
            {
                propertyBuilder.Property(m => m.Id).IsRequired();
                propertyBuilder.Property(m => m.CreatedAtUtc);
                propertyBuilder.Property(m => m.Text).IsRequired();

                propertyBuilder.OwnsOne(m => m.MessageType, propertyBuilder =>
                {
                    propertyBuilder.Property(m => m.Id).IsRequired();
                    propertyBuilder.Property(m => m.Name).IsRequired();
                });
            });

            builder.OwnsOne(m => m.MyRole, propertyBuilder =>
            {
                propertyBuilder.Property(m => m.Id);
                propertyBuilder.Property(m => m.Name);
            });


            builder.HasMany(e => e.Messages).WithOne().IsRequired();
        }
    }
}
