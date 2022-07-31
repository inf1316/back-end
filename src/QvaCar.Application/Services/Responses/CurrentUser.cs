using System;

namespace QvaCar.Application.Services
{
    public record CurrentUser
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Province { get; init; } = string.Empty;
        public string SubscriptionLevel { get; init; } = string.Empty;
    }
}
