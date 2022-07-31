using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
#nullable disable
    public class CarAdContactPhoneNumberTests
    {
        [Fact]
        public void Throws_Exception_When_Null()
        {
            Action action = () => { var r = new CarAdContactPhoneNumber(null); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_Empty()
        {
            Action action = () => { var r = new CarAdContactPhoneNumber(string.Empty); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_Spaces()
        {
            Action action = () => { var r = new CarAdContactPhoneNumber("   "); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_ToLong()
        {
            var str = GetStringOfLength(51);
            Action action = () => { var r = new CarAdContactPhoneNumber(str); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Can_Create()
        {
            var str = GetStringOfLength(20);
            var obj = new CarAdContactPhoneNumber(str);
            obj.Value.Should().Be(str);
        }
        private static string GetStringOfLength(int length) => new('a', length);
    }
#nullable enable
}
