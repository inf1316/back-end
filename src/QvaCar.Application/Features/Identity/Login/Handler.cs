using MediatR;
using QvaCar.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginCommandResponse>
    {
        private readonly IUserAuthService _userAuthService;

        public UserLoginCommandHandler
            (
                IUserAuthService userAuthService
            )
        {
            _userAuthService = userAuthService;
        }

        public async Task<UserLoginCommandResponse> Handle(UserLoginCommand command, CancellationToken cancellationToken)
        {
            var authResult = await _userAuthService.CheckUserLoginAsync(command.Email, command.Password, command.ReturnUrl, cancellationToken);
            return new UserLoginCommandResponse()
            {
                IsNativeClient = authResult.IsNativeClient,
                IsAuthContext = authResult.IsAuthContext,
                Success = authResult.Success,                
            };
        }
    }
}