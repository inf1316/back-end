using MediatR;
using QvaCar.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class UserLogoutCommandHandler : IRequestHandler<UserLogoutCommand, UserLogoutCommandResponse>
    {
        private readonly IUserAuthService _userAuthService;

        public UserLogoutCommandHandler(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task<UserLogoutCommandResponse> Handle(UserLogoutCommand command, CancellationToken cancellationToken)
        {
            var authResult = await _userAuthService.LogoutAsync(command.LogoutId, cancellationToken);
            return new UserLogoutCommandResponse()
            {
                AutomaticRedirectAfterSignOut = authResult.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = authResult.PostLogoutRedirectUri,
                ClientName = authResult.ClientName,
                SignOutIframeUrl = authResult.SignOutIframeUrl,
                LogoutId = authResult.LogoutId,
            };
        }
    }
}