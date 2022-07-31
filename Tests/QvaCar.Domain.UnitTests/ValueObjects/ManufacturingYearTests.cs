using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
    public class ManufacturingYearTests
    {
        [Fact]
        public void Throws_Exception_When_Negative()
        {
            Action action = () => { var r = new ManufacturingYear(-1); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_To_Old()
        {
            Action action = () => { var r = new ManufacturingYear(1899); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Can_Create_When_Limit()
        {
            var value = 1900;
            var obj = Kilometers.FromKilometers(value);
            obj.Value.Should().Be(value);
        }

        [Fact]
        public void Can_Create_When_Is_After_Limit()
        {
            var value = 1901;
            var obj = Kilometers.FromKilometers(value);
            obj.Value.Should().Be(value);
        }
    }
}
