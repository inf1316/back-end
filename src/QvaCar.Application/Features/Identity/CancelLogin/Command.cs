using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record CancelLoginCommand : IRequest<CancelLoginCommandResponse>
    {
        public string ReturnUrl { get; init; } = string.Empty;
    }
}
