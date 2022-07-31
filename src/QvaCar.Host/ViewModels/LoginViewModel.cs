namespace QvaCar.Web.ViewModels
{
    public record LoginViewModel
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string? ReturnUrl { get; init; }
    }
}
