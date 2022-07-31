using FluentValidation.TestHelper;
using QvaCar.Application.Features.Identity;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.Identity
{
    public class WhenValidatingGetRecoveryPasswordCommand
    {
        private readonly GetRecoveryPasswordCommandValidator validator;
        
        public WhenValidatingGetRecoveryPasswordCommand()
        {
            validator = new GetRecoveryPasswordCommandValidator();
        }

        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new GetRecoveryPasswordCommand();
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.UserEmail);        
        }

        [Fact]
        public void Returns_Validations_Errors_When_Email_Is_Invalid()
        {
            var command = new GetRecoveryPasswordCommand() { UserEmail = "invalid" };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.UserEmail);
        }

        [Fact]
        public void Does_Not_Return_Any_Error_When_Model_Is_Valid()
        {
            var command = new GetRecoveryPasswordCommand() 
            { 
                UserEmail = "valid@email.com",
            };
            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
