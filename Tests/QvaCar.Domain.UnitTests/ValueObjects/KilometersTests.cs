using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
    public class KilometersTests
    {
        [Fact]
        public void Throws_Exception_When_Negative()
        {
            Action action = () => Kilometers.FromKilometers(-1);
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Can_Create_When_0()
        {
            var value = 0;
            var obj = Kilometers.FromKilometers(value);
            obj.Value.Should().Be(value);
        }

        [Fact]
        public void Can_Create_When_Positive()
        {
            var value = 1;
            var obj = Kilometers.FromKilometers(value);
            obj.Value.Should().Be(value);
        }
    }
}
