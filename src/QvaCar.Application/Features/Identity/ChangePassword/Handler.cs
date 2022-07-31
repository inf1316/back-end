using MediatR;
using QvaCar.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordCommandResponse>
    {
        private readonly IUserAccountService _userAccountService;

        public ChangePasswordCommandHandler(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task<ChangePasswordCommandResponse> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var success = await _userAccountService.ChangePasswordAsync(command.Email, command.RecoveryPasswordToken, command.NewPassword, cancellationToken);
            return await Task.FromResult(new ChangePasswordCommandResponse() { Success = success });
        }
    }
}