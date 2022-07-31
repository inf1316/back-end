using FluentAssertions;
using QvaCar.Application.Exceptions;
using System;
using Xunit;

namespace QvaCar.Application.UnitTests.Common
{
    public class ValidateTest
    {
        #region Is Not Null
        [Fact]

        public void IsNotNull_Does_Not_Throws_Exception_When_True()
        {
            Action act = () => Validate.IsNotNull(1,"Prop","Desc");
            act.Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void IsNotNull_Throws_Exception_When_False()
        {
            Action act = () => Validate.IsNotNull(null, "Prop", "Desc");
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void IsNotNull_Throws_Exception_When_False_With_Custom_Message()
        {
            var errorMsg = "One or more validation failures have occurred.";
            ValidationException exGlobal = null;

            try
            {
                Validate.IsNotNull(null, "Prop", "Desc");
            }
            catch (ValidationException ex)
            { exGlobal = ex; }

            exGlobal.Should().NotBeNull();
            exGlobal?.Message?.Should().Be(errorMsg);
        }

        [Fact]
        public void IsNotNull_Does_Not_Throws_Exception_When_True_With_Custom_Message()
        {
            Action act = () => Validate.IsNotNull(1, "Prop", "Desc");
            act.Should().NotThrow<ValidationException>();
        }
        #endregion     
    }
}
