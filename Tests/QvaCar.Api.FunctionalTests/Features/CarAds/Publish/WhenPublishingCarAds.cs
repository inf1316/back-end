using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Api.FunctionalTests.SeedWork;
using QvaCar.Api.FunctionalTests.Shared.CarAds;
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
    public class WhenPublishingCarAds
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;
        public WhenPublishingCarAds(TestServerFixture given)
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
            var invalidId = Guid.NewGuid();
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));

            var requestUrl = ApiHelper.Put.Publish(invalidId);
            var response = await Given.Server
                                     .CreateClient()
                                     .PutAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Id_Dont_Exist()
        {
            var invalidId = Guid.NewGuid();
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));

            var requestUrl = ApiHelper.Put.Publish(invalidId);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .PutAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Publishing_Another_User_Ad()
        {
            var invalidUserId = Guid.NewGuid();
            var carInDb = await GivenDefaultAdInRepositoryForUser(invalidUserId, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));

            var requestUrl = ApiHelper.Put.Publish(carInDb.Id);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .PutAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Publish_A_Draft_Ad()
        {
            var clockNow = new DateTime(2020, 01, 03, 12, 20, 01);
            var originalCarInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));
            Given.AssumeClockNowAt(clockNow);

            await PutAndPublish(ValidUser,originalCarInDb.Id);

            var afterCarsInDb = await Given.GetAllCarsAdsInRepository();
            afterCarsInDb.Should().NotBeNull().And.HaveCount(1);
            var carInDbAfter = afterCarsInDb.First();

            carInDbAfter.State.Should().Be(AdState.Published);
            carInDbAfter.UpdatedAt.Should().Be(clockNow);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Bad_Request_When_Publishing_An_Already_Published_Ad()
        {
            var clockNow = new DateTime(2020, 01, 03, 12, 20, 01);
            var originalCarInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Published, new DateTime(2021, 01, 02, 03, 04, 05));
            Given.AssumeClockNowAt(clockNow);

            var requestUrl = ApiHelper.Put.Publish(originalCarInDb.Id);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .PutAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Bad_Request_When_Publishing_An_Already_Unregistered_Ad()
        {
            var clockNow = new DateTime(2020, 01, 03, 12, 20, 01);
            var originalCarInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Unregistered, new DateTime(2021, 01, 02, 03, 04, 05));
            Given.AssumeClockNowAt(clockNow);

            var requestUrl = ApiHelper.Put.Publish(originalCarInDb.Id);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .PutAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        #region Helpers
        private async Task PutAndPublish(TestApiUser asUser, Guid id)
        {
            var requestUrl = ApiHelper.Put.Publish(id);
            var response = await Given.Server
                                  .CreateClient()
                                  .FromUser(asUser)
                                  .PutAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private async Task<CarAd> GivenDefaultAdInRepositoryForUser(Guid userId, AdState state, DateTime createdAt)
        {
            var priceInDollars = 1500;
            var province = Province.SantiagoDeCuba;
            var manufacturingYear = 1991;
            var kilometers = 101;
            var bodyType = CarBodyType.Familiar;
            var color = Color.Negro;
            var fuelType = FuelType.Gasolina;
            var gearboxType = GearboxType.Automatico;
            var description = "Some description 2";
            var contactPhoneNumber = "551012";
            var version = "Yaris";

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var carInDb = new CarAdBuilder()
                                  .WithCreatedAt(createdAt)
                                  .WithUserId(userId)
                                  .WithPrice(priceInDollars)
                                  .WithProvince(province)
                                  .WithManufacturingYear(manufacturingYear)
                                  .WithKilometers(kilometers)
                                  .WithBodyType(bodyType)
                                  .WithColor(color)
                                  .WithFuelType(fuelType)
                                  .WithGearboxType(gearboxType)
                                  .WithDescription(description)
                                  .WithContactPhoneNumber(contactPhoneNumber)
                                  .WithModelVersion(version)
                                  .WithExteriorTypes(exteriorTypes)
                                  .WithSafetyTypes(safetyTypes)
                                  .WithInsideTypes(insideTypes)
                                  .WithAdState(state)
                                  .Build();
            await Given.AssumeCarAdInRepository(carInDb);
            return carInDb;
        }

        private static class ApiHelper
        {
            public static class Put
            {
                public static string Publish(Guid id)
                {
                    return $"/api/car-ads/{id}/publish";
                }
            }
        }
        #endregion 
    }
}
