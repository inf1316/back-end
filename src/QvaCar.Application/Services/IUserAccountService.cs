using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Services
{
    public interface IUserAccountService
    {
        Task<bool> ChangePasswordAsync(string email, string recoveryPasswordToken, string newPassword, CancellationToken cancellationToken);
        Task<bool> ConfirmEmailAsync(Guid userId, string confirmationToken, CancellationToken cancellationToken);
        Task<string> GetRecoveryPasswordTokenAsync(string email, CancellationToken cancellationToken);
        Task<UserRegistrationServiceResponse> RegisterUserAsync(UserRegistrationServiceRequest request, CancellationToken cancellationToken);
    }
}
