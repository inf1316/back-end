namespace QvaCar.Web.ViewModels
{
    public record PasswordResetEmailConfirmationSentViewModel
    {
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
