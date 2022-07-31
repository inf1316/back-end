namespace QvaCar.Application.Services
{
    public record CancelLoginResponse
    {
        public bool IsNativeClient { get; init; }
        public bool IsAuthContext { get; init; }
    }
}
