using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record UserLoginCommand : IRequest<UserLoginCommandResponse>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
