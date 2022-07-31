using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
#nullable disable
    public class CarAdDescriptionTests
    {
        [Fact]
        public void Throws_Exception_When_Null()
        {
            Action action = () => { var r = new CarAdDescription(null); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_Empty()
        {
            Action action = () => { var r = new CarAdDescription(string.Empty); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_Spaces()
        {
            Action action = () => { var r = new CarAdDescription("   "); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_ToLong()
        {
            var str = GetStringOfLength(501);
            Action action = () => { var r = new CarAdDescription(str); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Can_Create()
        {
            var str = GetStringOfLength(500);
            var obj = new CarAdDescription(str);
            obj.Value.Should().Be(str);
        }
        private static string GetStringOfLength(int length) => new('a', length);
    }
#nullable enable
}
