using Microsoft.AspNetCore.Mvc;
using QvaCar.Web.ViewModels;
using System.Threading.Tasks;
using QvaCar.Web.Common;
using Microsoft.AspNetCore.Authorization;
using System;
using QvaCar.Application.Features.Identity;
using QvaCar.Application.Exceptions;
using System.Linq;

namespace QvaCar.Web.Features.Identity
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Route("account")]
    public class AccountController : UIControllerBase
    {
        public const string InvalidCredentialsErrorMessage = "Invalid username or password";

        #region Log In
        [HttpGet("login")]
        public IActionResult Login(string? returnUrl)
        {
            var vm = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Email = string.Empty,
            };
            return View(vm);
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string button)
        {
            var returnUrl = model.ReturnUrl ?? string.Empty;
            if (button != "login")
                return await CancelLoginAsync(returnUrl);

            if (!ModelState.IsValid)
            {
                var vm = new LoginViewModel
                {
                    ReturnUrl = model.ReturnUrl,
                    Email = model.Email,
                };
                return View(vm);
            }

            var loginCommand = new UserLoginCommand() { Email = model.Email, Password = model.Password };

            try
            {
                var loginResponse = await Mediator.Send(loginCommand);

                if (!loginResponse.Success)
                {
                    ModelState.AddModelError(string.Empty, InvalidCredentialsErrorMessage);
                    var vm = new LoginViewModel
                    {
                        ReturnUrl = model.ReturnUrl,
                        Email = model.Email,
                    };
                    return View(vm);
                }

                if (loginResponse.IsAuthContext)
                {
                    if (loginResponse.IsNativeClient)
                    {
                        return this.LoadingPage("Redirect", returnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors.Values.SelectMany(x => x))
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            if (Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                throw new Exception("invalid return URL");
            }
        }
        private async Task<IActionResult> CancelLoginAsync(string returnUrl)
        {
            var command = new CancelLoginCommand() { ReturnUrl = returnUrl };
            var cancelLoginResponse = await Mediator.Send(command);

            if (!cancelLoginResponse.IsAuthContext)
                return Redirect("~/");

            if (cancelLoginResponse.IsNativeClient)
                return this.LoadingPage("Redirect", returnUrl);

            return Redirect(returnUrl);
        }
        #endregion

        #region Log-Out
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = new LogoutInputModel { LogoutId = logoutId, };
            return await Logout(vm);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var command = new UserLogoutCommand() { LogoutId = model.LogoutId };
            var response = await Mediator.Send(command);

            var vm = new LoggedOutViewModel()
            {
                AutomaticRedirectAfterSignOut = response.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = response.PostLogoutRedirectUri,
                ClientName = response.ClientName,
                SignOutIframeUrl = response.SignOutIframeUrl,
                LogoutId = response.LogoutId,
            };

            return View("LoggedOut", vm);
        }
        #endregion

        #region AccessDenied
        [HttpGet("access-denied")]
        public IActionResult AccessDenied() => View();
        #endregion
    }
}
