namespace QvaCar.Application.Services
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        CurrentUser GetCurrentUser();
    }  
}
