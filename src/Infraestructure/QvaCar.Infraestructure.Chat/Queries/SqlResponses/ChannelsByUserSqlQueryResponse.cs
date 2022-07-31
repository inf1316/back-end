using System;

namespace QvaCar.Infraestructure.Chat.Queries
{
    public record ChannelsByUserSqlQueryResponse
    {
        public Guid Id { get; init; }

        public Guid AboutAdId { get; init; }
        public string AboutAdModelVersion { get; init; } = string.Empty;
        public string? AboutAdMainImageFileName { get; init; }

        public Guid AnotherParticipantId { get; init; }
        public string AnotherParticipantFirstName { get; set; } = string.Empty;
        public string AnotherParticipantLastName { get; set; } = string.Empty;

        public long LastMessageId { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public string LastMessageText { get; init; } = string.Empty;
        public int LastMessageMessageTypeId { get; init; }
        public string LastMessageMessageTypeName { get; init; } = string.Empty;

        public int UserRoleId { get; init; }
        public string UserRoleName { get; init; } = string.Empty;
    }
}
