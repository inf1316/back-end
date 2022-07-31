using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using QvaCar.Application.Exceptions;
using QvaCar.Application.Features.Exceptions;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.Identity;
using QvaCar.Infraestructure.Identity.Configuration;
using QvaCar.Infraestructure.Identity.Models;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Identity.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserManager<QvaCarIdentityUser> _userManager;
        private readonly IMediator _mediator;

        public UserAccountService(UserManager<QvaCarIdentityUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<string> GetRecoveryPasswordTokenAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new UserNotFoundDomainException("There is no account for that email");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<bool> ChangePasswordAsync(string email, string recoveryPasswordToken, string newPassword, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new UserNotFoundDomainException("There is no account for that email");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, recoveryPasswordToken, newPassword);

            if (!resetPassResult.Succeeded)
            {
                throw new RegisterUserException(resetPassResult.Errors.Select(x => x.Description).ToArray());
            }

            return resetPassResult.Succeeded;
        }

        public async Task<UserRegistrationServiceResponse> RegisterUserAsync(UserRegistrationServiceRequest request, CancellationToken cancellationToken)
        {
            var id = request.User.Id;
            var email = request.User.Email;
            var firstName = request.User.FirstName;
            var lastName = request.User.LastName;
            var age = request.User.Age;
            var provinceId = request.User.ProvinceId;
            var address = request.User.Address;
            var provinceName = Enumeration.GetAll<Province>().FirstOrDefault(x => x.Id == provinceId)?.Name;
            var password = request.Password;
            var emailConfirmed = request.SkipEmailConfirmation;
            var createAsAdmin = request.CreateAsAdmin;
            var subscriptionLevel = request.User.SubscriptionLevel;

            if (provinceName is null)
                throw new ArgumentNullException("Invalid Province");

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                throw new UserAlreadyExistDomainException($"There is already a user registered for '{email}'.");

            var userToCreate = new QvaCarIdentityUser
            {
                Id = id,
                UserName = email,
                Email = email,
                EmailConfirmed = emailConfirmed,
                Age = age,
                ProvinceId = provinceId,
                Address = address,
                FirstName = firstName,
                LastName = lastName,
                SubscriptionLevelId = subscriptionLevel.Id,
            };

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Address, address),
                new Claim(JwtClaimTypes.GivenName, firstName),
                new Claim(JwtClaimTypes.FamilyName, lastName),
                new Claim(JwtClaimTypes.Address, age.ToString()),
                new Claim(QvaCarClaims.Province, provinceName),
                new Claim(QvaCarClaims.SubscriptionLevel, subscriptionLevel.Name),
            };

            var createResult = await _userManager.CreateAsync(userToCreate, password);
            await DispatchDomainEvents(request.User);

            if (!createResult.Succeeded)
            {
                throw new RegisterUserException(createResult.Errors.Select(x => x.Description).ToArray());
            }

            var createdUser = await _userManager.FindByEmailAsync(email);

            var roleName = createAsAdmin ? QvaCarIdentityRole.AdminRole.Name : QvaCarIdentityRole.RegularUserRole.Name;
            var roleResult = await _userManager.AddToRoleAsync(createdUser, roleName);
            var claimsResult = await _userManager.AddClaimsAsync(createdUser, claims);

            if (!roleResult.Succeeded)
            {
                throw new Exception("Fail to create your User");
            }

            if (!claimsResult.Succeeded)
            {
                throw new Exception("Fail to create your User");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(createdUser);
            return new UserRegistrationServiceResponse()
            {
                AccountConfirmationToken = token,
            };
        }

        public async Task<bool> ConfirmEmailAsync(Guid userId, string confirmationToken, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            return result.Succeeded;
        }

        private async Task DispatchDomainEvents(QvaCarUser user)
        {
            var events = user.DomainEvents.ToArray();
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
            user.ClearEvents();
        }
    }
}
