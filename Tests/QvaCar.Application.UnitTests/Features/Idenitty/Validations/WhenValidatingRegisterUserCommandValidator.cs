using FluentValidation.TestHelper;
using QvaCar.Application.Features.Identity;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.Identity
{
    public class WhenValidatingRegisterUserCommandValidator
    {
        private readonly RegisterUserCommandValidator validator;
       
        public WhenValidatingRegisterUserCommandValidator()
        {
            validator = new RegisterUserCommandValidator();
        }

        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new RegisterUserCommand();
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Email);
            result.ShouldHaveValidationErrorFor(cmd => cmd.FirstName);
            result.ShouldHaveValidationErrorFor(cmd => cmd.LastName);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Age);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Address);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ProvinceId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Password);
        }

        [Fact]
        public void Returns_Validations_Errors_When_Email_Is_Invalid()
        {
            var command = new RegisterUserCommand() { Email = "invalid" };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Email);
        }

        [Fact]
        public void Returns_Validations_Errors_When_Age_Is_Negative()
        {
            var command = new RegisterUserCommand() { Age = -1 };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Age);
        }

        [Fact]
        public void Returns_Validations_Errors_When_Province_Is_Negative()
        {
            var command = new RegisterUserCommand() { ProvinceId = -1 };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.ProvinceId);
        }
     
        [Fact]
        public void Does_Not_Return_Any_Error_When_Model_Is_Valid()
        {
            var command = new RegisterUserCommand()
            {
                Email = "valid@email.com",
                FirstName  = "FN",
                LastName = "LN",
                Age = 18,
                Address = "AD",
                ProvinceId = 1,
                Password = "Pass"
            };
            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
