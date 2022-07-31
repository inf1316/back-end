using FluentValidation;

namespace QvaCar.Application.Features.Identity
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(v => v.Email)
              .NotEmpty()
              .WithMessage("The email cannot be empty.");

            RuleFor(v => v.Email)
               .EmailAddress()
               .WithMessage("Invalid email address.");

            RuleFor(v => v.RecoveryPasswordToken)
              .NotEmpty()
              .WithMessage("The password token cannot be empty.");

            RuleFor(v => v.NewPassword)
              .NotEmpty()
              .WithMessage("The password token cannot be empty.");
        }
    }   
}
