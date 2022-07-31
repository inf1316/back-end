using FluentValidation.TestHelper;
using QvaCar.Application.Features.CarAds;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.CarAds
{
    public class WhenValidatingUpdateCarAdCommand
    {
        private readonly UpdateCarAdCommandValidator validator;
        public WhenValidatingUpdateCarAdCommand()
        {
            validator = new UpdateCarAdCommandValidator();
        }


        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new UpdateCarAdCommand();
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

            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Kilometers);
        }

        [Fact]
        public void Returns_Validations_Error_For_Description_When_Is_To_Long()
        {
            var command = new UpdateCarAdCommand()
            {
                Description = GetStringOfLength(501)
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Contact_Phone_Number_When_Is_To_Long()
        {
            var command = new UpdateCarAdCommand()
            {
                ContactPhoneNumber = GetStringOfLength(21)
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Model_Version_When_Is_To_Long()
        {
            var command = new UpdateCarAdCommand()
            {
                ModelVersion = GetStringOfLength(51)
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Price_When_Is_To_Short()
        {
            var command = new UpdateCarAdCommand()
            {
                Price = 99
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }

        [Fact]
        public void Returns_Validations_Error_For_Manufacturing_Year_When_Is_To_Short()
        {
            var command = new UpdateCarAdCommand()
            {
                ManufacturingYear = 1900
            };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Description);
        }


        private static string GetStringOfLength(int length) => new('a', length);
    }
}
