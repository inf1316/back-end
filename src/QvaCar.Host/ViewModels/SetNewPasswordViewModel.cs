namespace QvaCar.Web.ViewModels
{
    public record SetNewPasswordViewModel
    {
        public string Password { get; init; } = string.Empty;
        public string ConfirmPassword { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
}
