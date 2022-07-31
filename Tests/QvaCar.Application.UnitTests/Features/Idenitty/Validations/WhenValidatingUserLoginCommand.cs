using FluentValidation.TestHelper;
using QvaCar.Application.Features.Identity;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.Identity
{
    public class WhenValidatingUserLoginCommand
    {
        private readonly UserLoginCommandValidator validator;
        
        public WhenValidatingUserLoginCommand()
        {
            validator = new UserLoginCommandValidator();
        }

        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new UserLoginCommand();
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Email);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Password);
        }

        [Fact]
        public void Returns_Validations_Errors_When_Email_Is_Invalid()
        {
            var command = new UserLoginCommand() { Email = "invalid" };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Email);
        }

        [Fact]
        public void Does_Not_Return_Any_Error_When_Model_Is_Valid()
        {
            var command = new UserLoginCommand() 
            {
                Email = "valid@email.com",
                Password = "pass"
            };
            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
