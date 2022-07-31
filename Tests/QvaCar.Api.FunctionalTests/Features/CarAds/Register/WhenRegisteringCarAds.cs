using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Api.Features.CarAds;
using QvaCar.Api.FunctionalTests.SeedWork;
using QvaCar.Api.FunctionalTests.Shared.CarAds.Extensions;
using QvaCar.Api.FunctionalTests.Shared.Identity;
using QvaCar.Api.FunctionalTests.Shared.Mocks.Extensions;
using QvaCar.Domain.CarAds;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    [Collection(nameof(TestServerFixtureCollection))]
    public class WhenRegisteringCarAds
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;

        public WhenRegisteringCarAds(TestServerFixture given)
        {
            Given = given ?? throw new Exception("Null Server");
            ValidUser = new TestApiUserBuilder()
               .WithId(Guid.NewGuid())
               .WithFreeSubscriptionLevel()
               .Build();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Unauthorized_When_User_Is_Not_Logged_In()
        {
            var request = new RegisterCarAdRequestBuilder()
                            .Build();

            var requestUrl = ApiHelper.Post.RegisterCarAdUrl();
            var response = await Given.Server
                                     .CreateClient()
                                     .PostAsync(requestUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Bad_Request_When_Request_Is_Invalid()
        {
            var request = new RegisterCarAdRequestBuilder()
                            .Build();

            var requestUrl = ApiHelper.Post.RegisterCarAdUrl();
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .PostAsync(requestUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Create_Element_When_Request_Is_Correct()
        {
            var userId = ValidUser.Id;
            var priceInDollars = 1000;
            var province = Province.Cienfuegos;
            var manufacturingYear = 1990;
            var kilometers = 100;
            var bodyType = CarBodyType.Berlina;
            var color = Color.Blanco;
            var fuelType = FuelType.Diesel;
            var gearboxType = GearboxType.Manual;
            var description = "Some description";
            var contactPhoneNumber = "551011";
            var version = "Golf GTI";
            var clockNow = new DateTime(2021, 01, 02, 12, 20, 0);
            
            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires,  ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };
            var contactLocation = new RegisterCarAdCoordinateBuilder()
                .WithLatitude(20)
                .WithLongitude(25)
                .Build();

            var request = new RegisterCarAdRequestBuilder()
                            .WithPrice(priceInDollars)
                            .WithProvinceId(province.Id)
                            .WithManufacturingYear(manufacturingYear)
                            .WithKilometers(kilometers)
                            .WithBodyTypeId(bodyType.Id)
                            .WithColorId(color.Id)
                            .WithFuelTypeId(fuelType.Id)
                            .WithGearboxTypeId(gearboxType.Id)
                            .WithDescription(description)
                            .WithContactPhoneNumber(contactPhoneNumber)
                            .WithModelVersion(version)
                            .WithContactLocation(contactLocation)
                            .WithExteriorTypes(exteriorTypes)
                            .WithInsideTypes(insideTypes)
                            .WithSafetyTypes(safetyTypes)
                            .Build();
            
            Given.AssumeClockNowAt(clockNow);
            var response = await PostRegisterCarAd(ValidUser,request);

            response.Should().NotBeNull();
            response.Id.Should().NotBeEquivalentTo(default);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);

            var carInDb = carAdsInDb.First();

            response.Should().NotBeNull();
            AssertCarAdResponse(carInDb, new Guid(response.Id), userId, clockNow, request);
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Create_Element_With_No_Location()
        {
            var userId = ValidUser.Id;
            var priceInDollars = 1000;
            var province = Province.Cienfuegos;
            var manufacturingYear = 1990;
            var kilometers = 100;
            var bodyType = CarBodyType.Berlina;
            var color = Color.Blanco;
            var fuelType = FuelType.Diesel;
            var gearboxType = GearboxType.Manual;
            var description = "Some description";
            var contactPhoneNumber = "551011";
            var version = "Golf GTI";
            var clockNow = new DateTime(2021, 01, 02, 12, 20, 0);

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var request = new RegisterCarAdRequestBuilder()
                            .WithPrice(priceInDollars)
                            .WithProvinceId(province.Id)
                            .WithManufacturingYear(manufacturingYear)
                            .WithKilometers(kilometers)
                            .WithBodyTypeId(bodyType.Id)
                            .WithColorId(color.Id)
                            .WithFuelTypeId(fuelType.Id)
                            .WithGearboxTypeId(gearboxType.Id)
                            .WithDescription(description)
                            .WithContactPhoneNumber(contactPhoneNumber)
                            .WithModelVersion(version)
                            .WithContactLocation(null)
                            .WithExteriorTypes(exteriorTypes)
                            .WithInsideTypes(insideTypes)
                            .WithSafetyTypes(safetyTypes)
                            .Build();

            Given.AssumeClockNowAt(clockNow);
            var response = await PostRegisterCarAd(ValidUser, request);

            response.Should().NotBeNull();
            response.Id.Should().NotBeEquivalentTo(default);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);

            var carInDb = carAdsInDb.First();

            response.Should().NotBeNull();
            AssertCarAdResponse(carInDb, new Guid(response.Id), userId, clockNow, request);
            carInDb.ContactLocation.Should().BeNull();
        }

        #region Helpers
        private async Task<RegisterCarAdResponse> PostRegisterCarAd(TestApiUser asUser, RegisterCarAdRequest request)
        {
            var requestUrl = ApiHelper.Post.RegisterCarAdUrl();
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(asUser)
                                     .PostAsync(requestUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseModel = await response.Deserialize<RegisterCarAdResponse>();
            return responseModel;
        }

        private static void AssertCarAdResponse(CarAd actual, Guid expectedId, Guid expectedUserId, DateTime clockNow, RegisterCarAdRequest expected)
        {
            actual.Should().NotBeNull();
            actual.Id.Should().Be(expectedId);
            actual.UserId.Should().Be(expectedUserId);
            actual.CreatedAt.Should().Be(clockNow);
            actual.UpdatedAt.Should().Be(clockNow);
            actual.State.Should().Be(AdState.Draft);

            actual.Price.PriceInDollars.Should().Be(expected.Price);
            actual.ManufacturingYear.Year.Should().Be(expected.ManufacturingYear);
            actual.Kilometers.Value.Should().Be(expected.Kilometers);
            actual.Description.Value.Should().Be(expected.Description);
            actual.ContactPhoneNumber.Value.Should().Be(expected.ContactPhoneNumber);
            actual.ModelVersion.Value.Should().Be(expected.ModelVersion);

            actual.Province.Id.Should().Be(expected.ProvinceId);
            actual.BodyType.Id.Should().Be(expected.BodyTypeId);
            actual.Color.Id.Should().Be(expected.ColorId);
            actual.FuelType.Id.Should().Be(expected.FuelTypeId);
            actual.GearboxType.Id.Should().Be(expected.GearboxTypeId);
            
            actual.ExteriorTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.ExteriorTypeIds);                        
            actual.SafetyTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.SafetyTypeIds);
            actual.InsideTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.InsideTypeIds);

            var actualLatitude = actual.ContactLocation?.Latitude;
            var actualLongitude = actual.ContactLocation?.Longitude;
            actualLatitude.Should().Be(expected?.ContactLocation?.Latitude);
            actualLongitude.Should().Be(expected?.ContactLocation?.Longitude);
        }

        private static class ApiHelper
        {
            public static class Post
            {
                public static string RegisterCarAdUrl()
                {
                    return $"/api/car-ads/register";
                }
            }
        }
        #endregion 
    }
}
