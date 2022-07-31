using MediatR;
using QvaCar.Application.Services;
using QvaCar.Domain.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IEmailService _emailService;
        private readonly UserSubscriptionLevel CreateSubscriptionLevel = UserSubscriptionLevel.Free;
        public RegisterUserCommandHandler(IUserAccountService userAccountService, IEmailService emailService)
        {
            _userAccountService = userAccountService;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var userToCreate = QvaCarUser.CreateNew(command.Email, command.FirstName, command.LastName, command.Age, command.Address, command.ProvinceId, CreateSubscriptionLevel);
            var createUserRequest = new UserRegistrationServiceRequest()
            {
                User = userToCreate,
                Password = command.Password,
                SkipEmailConfirmation = false,
            };
            var createUserResponse = await _userAccountService.RegisterUserAsync(createUserRequest, cancellationToken);

            string confirmAccountUrl = GetConfirmationAccountUrl(command.EmailConfirmationUrlTemplate, command.EmailConfirmationUrlUserIdParameterName, userToCreate.Id,
                                                              command.EmailConfirmationUrlTokenParameterName, createUserResponse.AccountConfirmationToken);
            await SendConfirmationEmailAsync(userToCreate.Email, confirmAccountUrl);

            return Unit.Value;
        }

        private static string GetConfirmationAccountUrl(string urlTemplate, string urlUserIdParameterName, Guid userId,
                                                 string urlTokenParameterName, string confirmAccountToken)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryParams[urlUserIdParameterName] = userId.ToString();
            queryParams[urlTokenParameterName] = confirmAccountToken;
            return $"{urlTemplate}?{queryParams}";
        }


        private async Task SendConfirmationEmailAsync(string userEmail, string confirmAccountUrl)
        {
            string body = $"<a href=\"{confirmAccountUrl}\">Confirm Email</a> or Copy this Url {confirmAccountUrl} in the browser.";
            await _emailService.SendEmailAsync("info@mydomain.com", userEmail, "Confirm your email address", body);
        }
    }
}
