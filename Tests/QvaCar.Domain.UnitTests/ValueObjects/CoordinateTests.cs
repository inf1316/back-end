using FluentAssertions;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using Xunit;

namespace QvaCar.Domain.UnitTests.Common
{
    public class CoordinateTests
    {
        [Theory]
        [InlineData(-91, false)]
        [InlineData(-90, true)]
        [InlineData(-89, true)]
        [InlineData(0, true)]
        [InlineData(89, true)]
        [InlineData(90, true)]
        [InlineData(91, false)]
        public void Throws_Exception_When_Latitude_Is_Out_Of_Bounds(int latitude, bool expectSuccess)
        {
            Func<Coordinate> action = () => Coordinate.FromLatLon(latitude, 15);

            if (expectSuccess)
            {
                var domainObject = action();
                domainObject.Should().NotBeNull();
                domainObject.Latitude.Should().Be(latitude);
            }
            else
            {
                action.Should().Throw<DomainValidationException>();
            }

        }

        [Theory]
        [InlineData(-181, false)]
        [InlineData(-180, true)]
        [InlineData(-179, true)]
        [InlineData(0, true)]
        [InlineData(179, true)]
        [InlineData(180, true)]
        [InlineData(181, false)]
        public void Throws_Exception_When_Longitude_Is_Out_Of_Bounds(int longitude, bool expectSuccess)
        {
            Func<Coordinate> action = () => Coordinate.FromLatLon(15, longitude);

            if (expectSuccess)
            {
                var domainObject = action();
                domainObject.Should().NotBeNull();
                domainObject.Longitude.Should().Be(longitude);
            }
            else
            {
                action.Should().Throw<DomainValidationException>();
            }

        }

        [Fact]
        public void Can_Create_When_Valid_Values_Are_Provided()
        {
            var expectedLatitude= 15;
            var expectedLongitude = 16;
            Func<Coordinate> action = () => Coordinate.FromLatLon(expectedLatitude, expectedLongitude);

            var domainObject = action();
            domainObject.Should().NotBeNull();
            domainObject.Latitude.Should().Be(expectedLatitude);
            domainObject.Longitude.Should().Be(expectedLongitude);
        }
    }
}
