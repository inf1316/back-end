using FluentValidation;

namespace QvaCar.Application.Features.Identity
{
    public class ConfirmAccountCommandValidator : AbstractValidator<ConfirmAccountCommand>
    {
        public ConfirmAccountCommandValidator()
        {
            RuleFor(v => v.UserId)
              .NotEmpty()
              .WithMessage("UserId cannot be empty.");

            RuleFor(v => v.ConfirmAccountToken)
               .NotEmpty()
               .WithMessage("Invalid token.");
        }
    }   
}
