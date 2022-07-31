using FluentValidation.TestHelper;
using QvaCar.Application.Features.CarAds;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.CarAds
{
    public class WhenValidatingRegisterCarAdCommand
    {
        private readonly RegisterCarAdCommandValidator validator;
        public WhenValidatingRegisterCarAdCommand()
        {
            validator = new RegisterCarAdCommandValidator();
        }


        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new RegisterCarAdCommand();
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ContactPhoneNumber);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ModelVersion);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Price);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ManufacturingYear);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ProvinceId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.BodyTypeId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ColorId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.FuelTypeId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.GearboxTypeId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ExteriorTypeIds);
            result.ShouldHaveValidationErrorFor(cmd => cmd.SafetyTypeIds);
            result.ShouldHaveValidationErrorFor(cmd => cmd.InsideTypeIds);

            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Kilometers);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.ContactLocation);
        }

        [Fact]
        public void Returns_Validations_Error_For_Description_When_Is_To_Long()
        {
            var command = new RegisterCarAdCommand()
            {
                Description = GetStringOfLength(501)
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Contact_Phone_Number_When_Is_To_Long()
        {
            var command = new RegisterCarAdCommand()
            {
                ContactPhoneNumber = GetStringOfLength(21)
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Model_Version_When_Is_To_Long()
        {
            var command = new RegisterCarAdCommand()
            {
                ModelVersion = GetStringOfLength(51)
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Price_When_Is_To_Short()
        {
            var command = new RegisterCarAdCommand()
            {
                Price = 99
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Manufacturing_Year_When_Is_To_Short()
        {
            var command = new RegisterCarAdCommand()
            {
                ManufacturingYear = 1900
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }


        private static string GetStringOfLength(int length) => new('a', length);
    }
}
