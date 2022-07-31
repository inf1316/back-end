namespace QvaCar.Web.ViewModels
{
    public record LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; init; } = true;
    }
}
