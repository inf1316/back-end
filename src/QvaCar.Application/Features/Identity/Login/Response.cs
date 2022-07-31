namespace QvaCar.Application.Features.Identity
{
    public record UserLoginCommandResponse
    {
        public bool Success { get; init; }
        public bool IsNativeClient { get; init; }
        public bool IsAuthContext { get; init; }
    }   
}