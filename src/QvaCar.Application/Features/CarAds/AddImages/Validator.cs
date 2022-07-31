using FluentValidation;
using System.IO;

namespace QvaCar.Application.Features.CarAds
{
    public class AddImagesToCarAdCommandValidator : AbstractValidator<AddImagesToCarAdCommand>
    {
        public AddImagesToCarAdCommandValidator()
        {
            RuleFor(v => v.Id)
              .NotEmpty()
              .WithMessage("The Id cannot be empty.");

            RuleForEach(v => v.Images)
                .SetValidator(new FileValidator());
        }
    }

    internal class FileValidator : AbstractValidator<ImageStream>
    {
        public FileValidator()
        {
            RuleFor(v => v.ContentType)
                .NotNull()
                .WithMessage("Files type is not set.");

            RuleFor(v => v.ContentType)
                .Must(v => 
                               v.Equals("image/jpeg")
                            || v.Equals("image/jpg")
                            || v.Equals("image/png")
                     )
                .WithMessage("Invalid File Type.");

            RuleFor(v => v.FileName)
              .NotEmpty()
              .WithMessage("FileName cannot be empty.");

            RuleFor(v => Path.GetExtension(v.FileName))
            .NotEmpty()
            .WithMessage("FileName extension cannot be empty.");
        }
    }
}
