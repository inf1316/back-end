namespace QvaCar.Application.Features.Identity
{
    public record CancelLoginCommandResponse
    {
        public bool IsNativeClient { get; init; }
        public bool IsAuthContext { get; init; }
    }   
}