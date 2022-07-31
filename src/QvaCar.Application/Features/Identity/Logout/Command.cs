using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record UserLogoutCommand : IRequest<UserLogoutCommandResponse>
    {
        public string LogoutId { get; init; } = string.Empty;
    }
}