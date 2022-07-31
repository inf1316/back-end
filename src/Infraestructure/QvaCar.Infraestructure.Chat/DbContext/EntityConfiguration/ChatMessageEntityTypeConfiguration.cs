using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QvaCar.Domain.Chat;

namespace QvaCar.Infraestructure.Chat
{
    public class ChatMessageEntityTypeConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable(EntityConfigurationConstants.Messages, EntityConfigurationConstants.ChatTableSchema);
            builder
                .HasKey(x => x.Id);
            builder
               .Property(p => p.Id)
               .ValueGeneratedNever();
            builder.Property(model => model.CorrelationId);
            builder.Property(model => model.CreatedAtUtc);
            builder.OwnsOne(m => m.MessageType, propertyBuilder =>
            {
                propertyBuilder.Property(m => m.Id).IsRequired();
                propertyBuilder.Property(m => m.Name).IsRequired();
            });
        }
    }
}
