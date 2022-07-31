using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record ChangePasswordCommand : IRequest<ChangePasswordCommandResponse> 
    {
        public string Email { get; init; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RecoveryPasswordToken { get; set; } = string.Empty;
    }
}
