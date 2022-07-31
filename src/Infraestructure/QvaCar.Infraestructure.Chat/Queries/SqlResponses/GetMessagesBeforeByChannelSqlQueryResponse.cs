using System;

namespace QvaCar.Infraestructure.Chat.Queries
{

    public record GetMessagesBeforeByChannelSqlQueryResponse
    {
        public long Id { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public string Text { get; init; } = string.Empty;
        public int MessageTypeId { get; init; }
        public string MessageTypeName { get; init; } = string.Empty;
    }
}
