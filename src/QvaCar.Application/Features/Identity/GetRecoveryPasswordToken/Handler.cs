using MediatR;
using QvaCar.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class GetRecoveryPasswordCommandHandler : IRequestHandler<GetRecoveryPasswordCommand>
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IEmailService _emailService;

        public GetRecoveryPasswordCommandHandler(IUserAccountService userAccountService, IEmailService emailService)
        {
            _userAccountService = userAccountService;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(GetRecoveryPasswordCommand command, CancellationToken cancellationToken)
        {
            var token = await _userAccountService.GetRecoveryPasswordTokenAsync(command.UserEmail, cancellationToken);

            var changePasswordUrl = BuildChangePasswordUrl(command.PasswordChangeUrlTemplate, command.PasswordChangeUrlUserEmailParameterName,
                                                           command.UserEmail, command.PasswordChangeUrlTokenParameterName, token);

            await SendTokenEmailAsync(command.UserEmail, changePasswordUrl);

            return Unit.Value;
        }

        private string BuildChangePasswordUrl(string urlTemplate, string urlEmailParameterName, string email,
                                              string urlTokenParameterName, string confirmAccountToken)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryParams[urlEmailParameterName] = email.ToString();
            queryParams[urlTokenParameterName] = confirmAccountToken;
            return $"{urlTemplate}?{queryParams}";
        }

        private async Task SendTokenEmailAsync(string userEmail, string changePasswordURl)
        {
            string body = $"<a href=\"{changePasswordURl}\" >Click to reset your password</a> or open in the browser {changePasswordURl}";
            await _emailService.SendEmailAsync("info@mydomain.com", userEmail, "Reset Your Account Password", body);
        }
    }
}
