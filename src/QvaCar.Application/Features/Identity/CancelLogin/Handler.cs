using MediatR;
using QvaCar.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class CancelLoginCommandHandler : IRequestHandler<CancelLoginCommand, CancelLoginCommandResponse>
    {
        private readonly IUserAuthService _userAuthService;

        public CancelLoginCommandHandler(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task<CancelLoginCommandResponse> Handle(CancelLoginCommand command, CancellationToken cancellationToken)
        {
            var authResult = await _userAuthService.CancelLoginAsync(command.ReturnUrl, cancellationToken);
            return new CancelLoginCommandResponse()
            {
                IsAuthContext = authResult.IsAuthContext,
                IsNativeClient = authResult.IsNativeClient,
            };
        }
    }
}