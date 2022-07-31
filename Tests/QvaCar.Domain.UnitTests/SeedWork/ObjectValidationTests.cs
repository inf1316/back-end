using FluentAssertions;
using QvaCar.Seedwork.Domain;
using Xunit;

namespace QvaCar.Domain.UnitTests.SeedWork
{
    public class ObjectValidationTests
    {
        #region Is Not Null
        [Fact]
        public void IsNotNull_Returns_True_For_Non_Null_Values()
        {
            var obj = 1;
            obj.IsNotNull().Should().BeTrue();
        }

        [Fact]
        public void IsNotNull_Returns_False_For_Null_Values()
        {
            string? obj = null;
            obj.IsNotNull().Should().BeFalse();
        }
        #endregion

        #region Is Null
        [Fact]
        public void IsNull_Returns_False_For_Non_Null_Values()
        {
            var obj = 1;
            obj.IsNull().Should().BeFalse();
        }

        [Fact]
        public void IsNull_Returns_True_For_Null_Values()
        {
            string? obj = null;
            obj.IsNull().Should().BeTrue();
        }
        #endregion
    }
}
