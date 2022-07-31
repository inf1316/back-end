using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Services
{
    public interface IUserAuthService
    {
        public Task<UserLoginResponse> CheckUserLoginAsync(string email, string userPassword, string returnUrl, CancellationToken cancellationToken);
        public Task<UserLogoutResponse> LogoutAsync(string logoutId, CancellationToken cancellationToken);
        Task<CancelLoginResponse> CancelLoginAsync(string returnUrl, CancellationToken cancellationToken);
    }
}
