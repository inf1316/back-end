using QvaCar.Domain.Identity;

namespace QvaCar.Application.Services
{
    public record UserRegistrationServiceRequest
    {
        public QvaCarUser User { get; init; }
        public string Password { get; init; } = string.Empty;
        public bool SkipEmailConfirmation { get; init; } = false;
        public bool CreateAsAdmin { get; init; }
    }
}
