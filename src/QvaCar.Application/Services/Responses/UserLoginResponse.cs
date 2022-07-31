namespace QvaCar.Application.Services
{
    public record UserLoginResponse
    {
        public bool Success { get; init; }
        public bool IsNativeClient { get; init; }
        public bool IsAuthContext { get; init; }

    }
}
