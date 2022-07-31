using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QvaCar.Application.Services;
using QvaCar.Infraestructure.Identity.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Identity.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<QvaCarIdentityUser> _userManager;
        private readonly SignInManager<QvaCarIdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventService _events;

        public UserAuthService(
                                 UserManager<QvaCarIdentityUser> userManager,
                                 SignInManager<QvaCarIdentityUser> signInManager,
                                 IIdentityServerInteractionService interaction,
                                 IHttpContextAccessor httpContextAccessor,
                                 IEventService events
                              )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _httpContextAccessor = httpContextAccessor;
            _events = events;
        }

        public async Task<UserLoginResponse> CheckUserLoginAsync(string email, string userPassword, string returnUrl, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return new UserLoginResponse { Success = false };

            string userFullName = $"{user.FirstName} {user.LastName}";
            var result = await _signInManager.PasswordSignInAsync(user.UserName, userPassword, true, lockoutOnFailure: true);
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (!result.Succeeded)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(email, "invalid credentials", clientId: context?.Client.ClientId));
                return new UserLoginResponse { Success = false };
            }

            await _events.RaiseAsync(new UserLoginSuccessEvent(email, user.Id.ToString(), userFullName, clientId: context?.Client.ClientId));

            bool isNativeClient = context is not null && IsNativeClient(context);
            return new UserLoginResponse
            {
                Success = result.Succeeded,
                IsNativeClient = isNativeClient,
                IsAuthContext = context is not null,
            };
        }

        public async Task<UserLogoutResponse> LogoutAsync(string logoutId, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()));
            }
            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            var response = new UserLogoutResponse
            {
                AutomaticRedirectAfterSignOut = true,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            return response;
        }

        public async Task<CancelLoginResponse> CancelLoginAsync(string returnUrl, CancellationToken cancellationToken)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            bool isNativeClient = context is not null && IsNativeClient(context);

            if (context != null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
            }
            return new CancelLoginResponse()
            {
                IsNativeClient = isNativeClient,
                IsAuthContext = context is not null,
            };
        }

        private static bool IsNativeClient(AuthorizationRequest context)
        {
            return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
               && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
        }
    }
}
