using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record GetRecoveryPasswordCommand : IRequest 
    {
        public string UserEmail { get; init; } = string.Empty;

        public string PasswordChangeUrlTemplate = string.Empty;
        public string PasswordChangeUrlUserEmailParameterName = string.Empty;
        public string PasswordChangeUrlTokenParameterName = string.Empty;
    }
}
