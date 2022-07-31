using Microsoft.AspNetCore.Mvc;
using QvaCar.Web.ViewModels;
using System.Threading.Tasks;
using QvaCar.Application.Features.Identity;
using QvaCar.Web.Common;
using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Rendering;
using QvaCar.Application.Exceptions;
using System.Linq;
using QvaCar.Application.Features.Exceptions;

namespace QvaCar.Web.Features.Identity
{
    [Route("user-registration")]
    public class UserRegistrationController : UIControllerBase
    {
        [HttpGet("register")]
        public async Task<IActionResult> RegisterUserAsync(string? returnUrl)
        {
            var vm = new RegisterUserViewModel()
            {
                ReturnUrl = returnUrl,
            };
            return await ShowRegisterViewModel(vm);
        }

        
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return await ShowRegisterViewModel(model);
            }
                

            var confirmationLink = Url.ActionLink("ConfirmEmail");
            var command = new RegisterUserCommand()
            {
                Email = model.Email,
                Age = model.Age,
                ProvinceId = model.ProvinceId,
                Address = model.Address,
                FirstName = model.GivenName,
                LastName = model.FamilyName,
                Password = model.Password,
                EmailConfirmationUrlTemplate = confirmationLink,
                EmailConfirmationUrlTokenParameterName = "token",
                EmailConfirmationUrlUserIdParameterName = "userId",
            };

            try
            {
                await Mediator.Send(command, cancellationToken);
                return RedirectToAction("RegistrationCodeSent", new { returnUrl = model.ReturnUrl });
            }
            catch(ValidationException ex)
            {
                foreach (var error in ex.Errors.Values.SelectMany(x=>x))
                {
                    ModelState.AddModelError("", error);
                } 
            }
            catch(UserAlreadyExistDomainException ex)
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
                ModelState.AddModelError("", "Fail to create your User");
            }
            return await ShowRegisterViewModel(model);
        }

        [HttpGet("code-sent")]
        public IActionResult RegistrationCodeSentAsync(string returnUrl)
        {
            return View(new RegistrationCodeSentViewModel() { ReturnUrl = returnUrl });
        }

        [HttpGet("account-confirm")]
        public async Task<IActionResult> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken)
        {
            try
            {
                var command = new ConfirmAccountCommand() { UserId = userId, ConfirmAccountToken = token };
                var response = await Mediator.Send(command, cancellationToken);
                if (response.Success)
                    return View("EmailConfirmed");
            }
            catch { }
            return View("InvalidToken");
        }


        private async Task<IActionResult> ShowRegisterViewModel(RegisterUserViewModel viewModel)
        {
            var viemModelData = await Mediator.Send(new ViewLoginQuery());

            var vm = viewModel with
            {
                Provinces = new SelectList(viemModelData.Provinces, nameof(BaseReferenceDataItem.Id), nameof(BaseReferenceDataItem.Name))
            };
            
            return View(vm);
        }
    }
}
