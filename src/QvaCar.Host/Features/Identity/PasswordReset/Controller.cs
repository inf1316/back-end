using Microsoft.AspNetCore.Mvc;
using QvaCar.Web.ViewModels;
using System.Threading.Tasks;
using QvaCar.Application.Features.Identity;
using QvaCar.Web.Common;
using System.Threading;
using QvaCar.Application.Exceptions;
using System.Linq;

namespace QvaCar.Web.Features.Identity
{
    [Route("password-reset")]
    public class PasswordResetController : UIControllerBase
    {
        private const string ControllerName = "PasswordReset";

        [HttpGet("confirm-account")]
        public IActionResult ForgotPassword(string? returnUrl)
        {
            var viewModel = new PasswordResetViewModel() { ReturnUrl = returnUrl, };
            return View(viewModel);
        }

        [HttpPost("confirm-account")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordAsync(PasswordResetViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var command = new GetRecoveryPasswordCommand()
                {
                    UserEmail = viewModel.Email,
                    PasswordChangeUrlTemplate = Url.ActionLink(nameof(SetNewPassword), ControllerName),
                    PasswordChangeUrlUserEmailParameterName = "email",
                    PasswordChangeUrlTokenParameterName = "token",
                };

                await Mediator.Send(command, cancellationToken);
                return View("ForgotPasswordConfirmation", new PasswordResetEmailConfirmationSentViewModel { ReturnUrl = viewModel.ReturnUrl });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors.Values.SelectMany(x => x))
                {
                    ModelState.AddModelError("", error);
                }
            }
            catch (UserNotFoundDomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong");
            }
            return View(viewModel);
        }

        [HttpGet("new-password")]
        public IActionResult SetNewPassword(string email, string token)
        {
            var viewModel = new SetNewPasswordViewModel() { Email = email, Token = token };
            return View(viewModel);
        }

        [HttpPost("new-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetNewPasswordAsync(SetNewPasswordViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!viewModel.Password.Equals(viewModel.ConfirmPassword))
                ModelState.AddModelError(nameof(viewModel.Password),"Password Dont Match");
            
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var command = new ChangePasswordCommand()
                {
                    Email = viewModel.Email,
                    NewPassword = viewModel.Password,
                    RecoveryPasswordToken = viewModel.Token,
                };
                var passworResult = await Mediator.Send(command, cancellationToken);

                if (passworResult.Success)
                    return RedirectToAction(nameof(PasswordResetDone));
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors.Values.SelectMany(x => x))
                {
                    ModelState.AddModelError("", error);
                }
            }
            catch (UserNotFoundDomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (RegisterUserException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong");
            }

            return View(viewModel);
        }

        [HttpGet("new-password-done")]
        public IActionResult PasswordResetDone() => View();
    }
}
