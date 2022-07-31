using FluentValidation.TestHelper;
using QvaCar.Application.Features.Identity;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.Identity
{
    public class WhenValidatingChangingPasswordCommand
    {
        private readonly ChangePasswordCommandValidator validator;
        
        public WhenValidatingChangingPasswordCommand()
        {
            validator = new ChangePasswordCommandValidator();
        }

        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new ChangePasswordCommand();
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Email);
            result.ShouldHaveValidationErrorFor(cmd => cmd.NewPassword);
            result.ShouldHaveValidationErrorFor(cmd => cmd.RecoveryPasswordToken);
        }

        [Fact]
        public void Returns_Validations_Errors_When_Email_Is_Invalid()
        {
            var command = new ChangePasswordCommand() { Email = "invalid" };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Email);
        }

        [Fact]
        public void Does_Not_Return_Any_Error_When_Model_Is_Valid()
        {
            var command = new ChangePasswordCommand() 
            { 
                Email = "valid@email.com",
                NewPassword = "ABC",
                RecoveryPasswordToken = "RP",                
            };
            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
