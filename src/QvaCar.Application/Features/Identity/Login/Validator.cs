using FluentValidation;

namespace QvaCar.Application.Features.Identity
{
    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(v => v.Email)
              .NotEmpty()
              .WithMessage("The email cannot be empty.");

            RuleFor(v => v.Email)
                .EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(v => v.Password)
              .NotEmpty()
              .WithMessage("The password cannot be empty.");
        }
    }   
}
