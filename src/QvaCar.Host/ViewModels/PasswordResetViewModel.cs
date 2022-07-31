namespace QvaCar.Web.ViewModels
{
    public record PasswordResetViewModel
    {
        public string Email { get; init; } = string.Empty;
        public string? ReturnUrl { get; init; } = string.Empty;
    }
}
