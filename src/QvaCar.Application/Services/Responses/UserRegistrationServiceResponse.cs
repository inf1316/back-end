namespace QvaCar.Application.Services
{
    public record UserRegistrationServiceResponse
    {
        public string AccountConfirmationToken { get; init; } = string.Empty;
    }
}
