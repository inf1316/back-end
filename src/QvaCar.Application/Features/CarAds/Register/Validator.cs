using FluentValidation;

namespace QvaCar.Application.Features.CarAds
{
    public class RegisterCarAdCommandValidator : AbstractValidator<RegisterCarAdCommand>
    {
        public RegisterCarAdCommandValidator()
        {
            RuleFor(v => v.Description)
                .NotEmpty()
                .WithMessage("The description cannot be empty.");

            RuleFor(v => v.Description)
                .MaximumLength(500)
                .WithMessage("The description cannot be more than 500 characters.");

            RuleFor(v => v.ContactPhoneNumber)
                .NotEmpty()
                .WithMessage("The phone cannot be empty.");

            RuleFor(v => v.ContactPhoneNumber)
                .MaximumLength(20)
                .WithMessage("The phone is too long.");

            RuleFor(v => v.ModelVersion)
                .NotEmpty()
                .WithMessage("The version of the model cannot be empty.");

            RuleFor(v => v.ModelVersion)
                .MaximumLength(50)
                .WithMessage("The version cannot be longer than 50 characters.");

            RuleFor(model => model.Price)
                .GreaterThanOrEqualTo(100)
                .WithMessage("The price cannot be that small.");

            RuleFor(model => model.ManufacturingYear)
                .GreaterThanOrEqualTo(1900)
                .WithMessage("Wrong year.");

            RuleFor(model => model.Kilometers)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Incorrect mileage.");

            RuleFor(model => model.ProvinceId)
                .GreaterThanOrEqualTo(1);

            RuleFor(model => model.BodyTypeId)
               .GreaterThanOrEqualTo(1)
               .WithMessage("You must select a Body Type.");

            RuleFor(model => model.ColorId)
               .GreaterThanOrEqualTo(1)
               .WithMessage("You must select a color.");

            RuleFor(model => model.FuelTypeId)
               .GreaterThanOrEqualTo(1)
               .WithMessage("You must select a fuel type.");

            RuleFor(model => model.GearboxTypeId)
               .GreaterThanOrEqualTo(1)
               .WithMessage("You must select an exchange rate.");

            RuleFor(model => model.SafetyTypeIds)
              .NotEmpty()  
              .WithMessage("Safety Type it cannot be null.");

            RuleFor(model => model.ExteriorTypeIds)
              .NotEmpty()
              .WithMessage("Exterior Type it cannot be null.");

            RuleFor(model => model.InsideTypeIds)
             .NotEmpty()
             .WithMessage("Inside Type it cannot be null.");
        }
    }

}
