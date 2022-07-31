using MediatR;
using QvaCar.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class ConfirmAccountCommandHandler : IRequestHandler<ConfirmAccountCommand, ConfirmAccountCommandResponse>
    {
        private readonly IUserAccountService _userAccountService;

        public ConfirmAccountCommandHandler(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task<ConfirmAccountCommandResponse> Handle(ConfirmAccountCommand command, CancellationToken cancellationToken)
        {
            var success = await _userAccountService.ConfirmEmailAsync(command.UserId, command.ConfirmAccountToken, cancellationToken);
            return await Task.FromResult(new ConfirmAccountCommandResponse() { Success = success });
        }
    }
}
