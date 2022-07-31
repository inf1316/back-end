using FluentValidation.TestHelper;
using QvaCar.Application.Features.Identity;
using System;
using Xunit;

namespace QvaCar.Application.UnitTests.Features.Identity
{
    public class WhenValidatingConfirmAccountCommand
    {
        private readonly ConfirmAccountCommandValidator validator;
        
        public WhenValidatingConfirmAccountCommand()
        {
            validator = new ConfirmAccountCommandValidator();
        }

        [Fact]
        public void Returns_Validations_Errors_For_All_Required_Properties()
        {
            var command = new ConfirmAccountCommand();
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.ConfirmAccountToken);
        }

        [Fact]
        public void Does_Not_Return_Any_Error_When_Model_Is_Valid()
        {
            var command = new ConfirmAccountCommand() 
            { 
                UserId = Guid.NewGuid(),
                ConfirmAccountToken = "Token"
            };
            var result = validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
