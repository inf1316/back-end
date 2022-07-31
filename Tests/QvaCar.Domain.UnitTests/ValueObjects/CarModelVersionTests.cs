using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
#nullable disable
    public class CarModelVersionTests
    {
        [Fact]
        public void Throws_Exception_When_Null()
        {
            Action action = () => { var r = new ModelVersion(null); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_Empty()
        {
            Action action = () => { var r = new ModelVersion(string.Empty); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_Spaces()
        {
            Action action = () => { var r = new ModelVersion("   "); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Throws_Exception_When_ToLong()
        {
            var str = GetStringOfLength(51);
            Action action = () => { var r = new ModelVersion(str); };
            action.Should().Throw<DomainValidationException>();
        }

        [Fact]
        public void Can_Create()
        {
            var str = GetStringOfLength(50);
            var obj = new ModelVersion(str);
            obj.Value.Should().Be(str);
        }
        private static string GetStringOfLength(int length) => new('a', length);
    }
#nullable enable
}
