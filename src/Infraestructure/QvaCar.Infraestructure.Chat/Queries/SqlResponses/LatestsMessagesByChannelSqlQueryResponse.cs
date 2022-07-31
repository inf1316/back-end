using System;

namespace QvaCar.Infraestructure.Chat.Queries
{
    public record LatestsMessagesByChannelSqlQueryResponse
    {
        public long Id { get; init; }
        public DateTime CreatedAtUtc { get; private set; }
        public string Text { get; init; } = string.Empty;
        public int MessageTypeId { get; init; }
        public string MessageTypeName { get; init; } = string.Empty;
    }
}
