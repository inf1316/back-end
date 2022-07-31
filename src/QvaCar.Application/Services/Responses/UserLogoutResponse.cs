namespace QvaCar.Application.Services
{
    public record UserLogoutResponse
    {
        public string PostLogoutRedirectUri { get; init; } = string.Empty;
        public string ClientName { get; init; } = string.Empty;
        public string SignOutIframeUrl { get; init; } = string.Empty;
        public bool AutomaticRedirectAfterSignOut { get; init; }
        public string LogoutId { get; init; } = string.Empty;
    }
}
