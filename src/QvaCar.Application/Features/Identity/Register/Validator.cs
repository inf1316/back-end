using FluentValidation;

namespace QvaCar.Application.Features.Identity
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
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

            RuleFor(v => v.FirstName)
                .NotEmpty()
                .WithMessage("The first name cannot be empty.");

            RuleFor(v => v.LastName)
                .NotEmpty()
                .WithMessage("The last name cannot be empty.");

            RuleFor(v => v.Address)
                .NotEmpty()
                .WithMessage("The address cannot be empty.");

            RuleFor(v => v.Age)
                .GreaterThan(0)
                .WithMessage("Invalid age.");

            RuleFor(v => v.ProvinceId)
                .GreaterThan(0)
                .WithMessage("Invalid Province");
        }
    }
}
