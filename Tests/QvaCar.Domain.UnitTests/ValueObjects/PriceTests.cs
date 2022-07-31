using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
    public class PriceTests
    {
        [Fact]
        public void Throws_Exception_When_Negative()
        {
            Action action = () => Price.FromDollars(-1);
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_To_Small()
        {
            Action action = () => Price.FromDollars(99);
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Can_Create_When_Limit()
        {
            var value = 100L;
            var obj = Price.FromDollars(value);
            obj.PriceInDollars.Should().Be(value);
        }

        [Fact]
        public void Can_Create_When_Is_After_Limit()
        {
            var value = 101L;
            var obj = Price.FromDollars(value);
            obj.PriceInDollars.Should().Be(value);
        }
    }
}
