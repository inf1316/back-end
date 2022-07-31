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
    public class WhenUnregisteringCarAds
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;
        public WhenUnregisteringCarAds(TestServerFixture given)
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
            var userId = Guid.NewGuid();
            var invalidId = Guid.NewGuid();
            await GivenDefaultAdInRepositoryForUser(userId, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));

            var requestUrl = ApiHelper.Delete.UnregisterUrl(invalidId);
            var response = await Given.Server
                                     .CreateClient()
                                     .DeleteAsync(requestUrl);
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

            var requestUrl = ApiHelper.Delete.UnregisterUrl(invalidId);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .DeleteAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Unregistering_Another_User_Ad()
        {
            var invalidUserId = Guid.NewGuid();
            var carInDb = await GivenDefaultAdInRepositoryForUser(invalidUserId, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));

            var requestUrl = ApiHelper.Delete.UnregisterUrl(carInDb.Id);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .DeleteAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_UnRegister_A_Draft_Ad()
        {
            var clockNow = new DateTime(2020, 01, 03, 12, 20, 01);
            var originalCarInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));
            Given.AssumeClockNowAt(clockNow);

            await DeleteAndUnregister(ValidUser, originalCarInDb.Id);

            var afterCarsInDb = await Given.GetAllCarsAdsInRepository();
            afterCarsInDb.Should().NotBeNull().And.HaveCount(1);
            var carInDbAfter = afterCarsInDb.First();

            carInDbAfter.State.Should().Be(AdState.Unregistered);
            carInDbAfter.UpdatedAt.Should().Be(clockNow);
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_UnRegister_A_Published_Ad()
        {
            var clockNow = new DateTime(2020, 01, 03, 12, 20, 01);
            var originalCarInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Published, new DateTime(2021, 01, 02, 03, 04, 05));
            Given.AssumeClockNowAt(clockNow);

            await DeleteAndUnregister(ValidUser, originalCarInDb.Id);

            var afterCarsInDb = await Given.GetAllCarsAdsInRepository();
            afterCarsInDb.Should().NotBeNull().And.HaveCount(1);
            var carInDbAfter = afterCarsInDb.First();

            carInDbAfter.State.Should().Be(AdState.Unregistered);
            carInDbAfter.UpdatedAt.Should().Be(clockNow);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Bad_Request_When_Unregistering_An_Already_Unregistered_Ad()
        {
            var clockNow = new DateTime(2020, 01, 03, 12, 20, 01);
            var originalCarInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Unregistered, new DateTime(2021, 01, 02, 03, 04, 05));
            Given.AssumeClockNowAt(clockNow);

            var requestUrl = ApiHelper.Delete.UnregisterUrl(originalCarInDb.Id);
            var response = await Given.Server
                                     .CreateClient()
                                     .FromUser(ValidUser)
                                     .DeleteAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        #region Helpers
        private async Task DeleteAndUnregister(TestApiUser asUser, Guid id)
        {
            var requestUrl = ApiHelper.Delete.UnregisterUrl(id);
            var response = await Given.Server
                                  .CreateClient()
                                  .FromUser(asUser)
                                  .DeleteAsync(requestUrl);
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
                                  .WithAdState(state)
                                  .WithExteriorTypes(exteriorTypes)
                                  .WithInsideTypes(insideTypes)
                                  .WithSafetyTypes(safetyTypes)
                                  .Build();
            await Given.AssumeCarAdInRepository(carInDb);
            return carInDb;
        }

        private static class ApiHelper
        {
            public static class Delete
            {
                public static string UnregisterUrl(Guid id)
                {
                    return $"/api/car-ads/{id}/unregister";
                }
            }
        }
        #endregion 
    }
}
