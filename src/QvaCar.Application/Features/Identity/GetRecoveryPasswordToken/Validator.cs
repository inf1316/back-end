using FluentValidation;

namespace QvaCar.Application.Features.Identity
{
    public class GetRecoveryPasswordCommandValidator : AbstractValidator<GetRecoveryPasswordCommand>
    {
        public GetRecoveryPasswordCommandValidator()
        {
            RuleFor(v => v.UserEmail)
              .EmailAddress()
              .WithMessage("Invalid email address.");
        }
    }   
}
