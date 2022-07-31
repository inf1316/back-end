using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using QvaCar.Api.Features.CarAds;
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
    public class WhenUpdatingCarAds
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;

        public WhenUpdatingCarAds(TestServerFixture given)
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
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2020, 01, 02, 03, 04, 05));

            var invalidRequest = new UpdateCarAdRequestBuilder().Build();

            var requestUrl = ApiHelper.Put.UpdateCarAdUrl(Guid.NewGuid());
            var response = await Given.Server
                                      .CreateClient()
                                      .PutAsync(requestUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Bad_Request_When_Request_Is_Invalid()
        {
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2020, 01, 02, 03, 04, 05));

            var invalidRequest = new UpdateCarAdRequestBuilder().Build();

            var requestUrl = ApiHelper.Put.UpdateCarAdUrl(Guid.NewGuid());
            var response = await Given.Server
                                      .CreateClient()
                                      .FromUser(ValidUser)
                                      .PutAsync(requestUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Id_Not_Exists()
        {
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
            var wrongId = Guid.NewGuid();

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2020, 01, 02, 03, 04, 05));

            var validRequest = new UpdateCarAdRequestBuilder()
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
                            .WithExteriorTypes(exteriorTypes)
                            .WithInsideTypes(insideTypes)
                            .WithSafetyTypes(safetyTypes)
                            .Build();

            var requestUrl = ApiHelper.Put.UpdateCarAdUrl(wrongId);
            var response = await Given.Server
                                      .CreateClient()
                                      .FromUser(ValidUser)
                                      .PutAsync(requestUrl, validRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Car_Ad_Exists_But_Belongs_To_Another_User()
        {
            var invalidUserId = Guid.NewGuid();
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
            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2020, 01, 02, 03, 04, 05));
            var carInDB = await GivenDefaultAdInRepositoryForUser(invalidUserId, AdState.Draft, new DateTime(2020, 01, 02, 03, 04, 05));

            var validRequest = new UpdateCarAdRequestBuilder()
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
                            .WithExteriorTypes(exteriorTypes)
                            .WithInsideTypes(insideTypes)
                            .WithSafetyTypes(safetyTypes)
                            .Build();

            var requestUrl = ApiHelper.Put.UpdateCarAdUrl(carInDB.Id);
            var response = await Given.Server
                                       .CreateClient()
                                      .FromUser(ValidUser)
                                      .PutAsync(requestUrl, validRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Update_Element_When_Request_Is_Correct()
        {
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
            var state = AdState.Draft;
            var createdAt = new DateTime(2020, 01, 02, 03, 04, 05);
            var originalCar = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt);
            var clockNow = new DateTime(2020, 01, 02, 03, 04, 05);
            var contactLocation = new UpdateCarAdCoordinateBuilder()
                .WithLatitude(20)
                .WithLongitude(25)
                .Build();

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var request = new UpdateCarAdRequestBuilder()
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
                            .WithExteriorTypes(exteriorTypes)
                            .WithInsideTypes(insideTypes)
                            .WithSafetyTypes(safetyTypes)
                            .WithContactLocation(contactLocation)
                            .Build();

            Given.AssumeClockNowAt(clockNow);
            await PutUpdateCarAd(ValidUser, originalCar.Id, request);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);

            var carInDb = carAdsInDb.First();

            AssertCarAdResponse(carInDb, originalCar.Id, ValidUser.Id, createdAt, clockNow, state, request);
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Update_Element_When_Request_Is_Correct_And_Multiple_Elements_In_Database()
        {
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
            var clockNow = new DateTime(2020, 01, 02, 03, 04, 05);
            var state = AdState.Draft;
            var createdAt = new DateTime(2020, 01, 02, 03, 04, 05);

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var originalCar = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt);
            var updateCar = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt);

            var request = new UpdateCarAdRequestBuilder()
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
                            .WithExteriorTypes(exteriorTypes)
                            .WithInsideTypes(insideTypes)
                            .WithSafetyTypes(safetyTypes)
                            .Build();

            Given.AssumeClockNowAt(clockNow);
            await PutUpdateCarAd(ValidUser, updateCar.Id, request);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(2);

            var originalCarInDb = carAdsInDb.First(x => x.Id == originalCar.Id);
            var updatedCarInDb = carAdsInDb.First(x => x.Id == updateCar.Id);

            AssertCar(originalCarInDb, originalCar);
            AssertCarAdResponse(updatedCarInDb, updateCar.Id, ValidUser.Id, createdAt, clockNow, state, request);
        }

        [Fact]
        [ResetApplicationState]
        public async void Cannot_Update_Unregister_Ad()
        {
            var clockNow = new DateTime(2020, 01, 02, 03, 04, 05);
            var createdAt = new DateTime(2020, 01, 02, 03, 04, 05);
            var updateCar = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Unregistered, createdAt);

            var request = new UpdateCarAdRequestBuilder()
                            .WithPrice(updateCar.Price.PriceInDollars + 1)
                            .WithProvinceId(updateCar.Province.Id)
                            .WithManufacturingYear(updateCar.ManufacturingYear.Year)
                            .WithKilometers(updateCar.Kilometers.Value)
                            .WithBodyTypeId(updateCar.BodyType.Id)
                            .WithColorId(updateCar.Color.Id)
                            .WithFuelTypeId(updateCar.FuelType.Id)
                            .WithGearboxTypeId(updateCar.GearboxType.Id)
                            .WithDescription(updateCar.Description.Value)
                            .WithContactPhoneNumber(updateCar.ContactPhoneNumber.Value)
                            .WithModelVersion(updateCar.ModelVersion.Value)
                            .WithExteriorTypes(updateCar.ExteriorTypes.ToArray())
                            .WithInsideTypes(updateCar.InsideTypes.ToArray())
                            .WithSafetyTypes(updateCar.SafetyTypes.ToArray())
                            .Build();

            Given.AssumeClockNowAt(clockNow);
            var requestUrl = ApiHelper.Put.UpdateCarAdUrl(updateCar.Id);
            var response = await Given.Server
                                      .CreateClient()
                                      .FromUser(ValidUser)
                                      .PutAsync(requestUrl, request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Update_Draft_Ad()
        {
            var clockNow = new DateTime(2020, 01, 02, 03, 04, 05);
            var createdAt = new DateTime(2020, 01, 02, 03, 04, 05);
            var state = AdState.Draft;
            var updateCar = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt);

            var request = new UpdateCarAdRequestBuilder()
                            .WithPrice(updateCar.Price.PriceInDollars + 1)
                            .WithProvinceId(updateCar.Province.Id)
                            .WithManufacturingYear(updateCar.ManufacturingYear.Year)
                            .WithKilometers(updateCar.Kilometers.Value)
                            .WithBodyTypeId(updateCar.BodyType.Id)
                            .WithColorId(updateCar.Color.Id)
                            .WithFuelTypeId(updateCar.FuelType.Id)
                            .WithGearboxTypeId(updateCar.GearboxType.Id)
                            .WithDescription(updateCar.Description.Value)
                            .WithContactPhoneNumber(updateCar.ContactPhoneNumber.Value)
                            .WithModelVersion(updateCar.ModelVersion.Value)
                            .WithExteriorTypes(updateCar.ExteriorTypes.ToArray())
                            .WithSafetyTypes(updateCar.SafetyTypes.ToArray())
                            .WithInsideTypes(updateCar.InsideTypes.ToArray())
                            .Build();

            Given.AssumeClockNowAt(clockNow);
            await PutUpdateCarAd(ValidUser, updateCar.Id, request);

            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);

            var updatedCarInDb = carAdsInDb.First(x => x.Id == updateCar.Id);

            AssertCarAdResponse(updatedCarInDb, updateCar.Id, ValidUser.Id, createdAt, clockNow, state, request);
        }

        [Fact]
        [ResetApplicationState]
        public async void Can_Update_Published_Ad()
        {
            var clockNow = new DateTime(2020, 01, 02, 03, 04, 05);
            var createdAt = new DateTime(2020, 01, 02, 03, 04, 05);
            var state = AdState.Published;
            var updateCar = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt);

            var request = new UpdateCarAdRequestBuilder()
                            .WithPrice(updateCar.Price.PriceInDollars + 1)
                            .WithProvinceId(updateCar.Province.Id)
                            .WithManufacturingYear(updateCar.ManufacturingYear.Year)
                            .WithKilometers(updateCar.Kilometers.Value)
                            .WithBodyTypeId(updateCar.BodyType.Id)
                            .WithColorId(updateCar.Color.Id)
                            .WithFuelTypeId(updateCar.FuelType.Id)
                            .WithGearboxTypeId(updateCar.GearboxType.Id)
                            .WithDescription(updateCar.Description.Value)
                            .WithContactPhoneNumber(updateCar.ContactPhoneNumber.Value)
                            .WithModelVersion(updateCar.ModelVersion.Value)
                            .WithExteriorTypes(updateCar.ExteriorTypes.ToArray())
                            .WithSafetyTypes(updateCar.SafetyTypes.ToArray())
                            .WithInsideTypes(updateCar.InsideTypes.ToArray())
                            .Build();

            Given.AssumeClockNowAt(clockNow);
            await PutUpdateCarAd(ValidUser, updateCar.Id, request);


            var carAdsInDb = await Given.GetAllCarsAdsInRepository();
            carAdsInDb.Should().NotBeNull().And.HaveCount(1);

            var updatedCarInDb = carAdsInDb.First(x => x.Id == updateCar.Id);

            AssertCarAdResponse(updatedCarInDb, updateCar.Id, ValidUser.Id, createdAt, clockNow, state, request);
        }

        #region Helpers
        private async Task PutUpdateCarAd(TestApiUser asUser, Guid id, UpdateCarAdRequest request)
        {
            var requestUrl = ApiHelper.Put.UpdateCarAdUrl(id);
            var response = await Given.Server
                                      .CreateClient()
                                      .FromUser(asUser)
                                      .PutAsync(requestUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private static void AssertCarAdResponse(
                                                    CarAd actual,
                                                    Guid expectedId, Guid expectedUserId,
                                                    DateTime createdAt,
                                                    DateTime clockNow,
                                                    AdState expectedState,
                                                    UpdateCarAdRequest expected
                                                )
        {
            actual.Should().NotBeNull();
            actual.Id.Should().Be(expectedId);
            actual.UserId.Should().Be(expectedUserId);
            actual.CreatedAt.Should().Be(createdAt);
            actual.UpdatedAt.Should().Be(clockNow);
            actual.State.Should().Be(expectedState);

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
            actual.InsideTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.InsideTypeIds);
            actual.SafetyTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.SafetyTypeIds);
           
            var actualLatitude = actual.ContactLocation?.Latitude;
            var actualLongitude = actual.ContactLocation?.Longitude;
            actualLatitude.Should().Be(expected?.ContactLocation?.Latitude);
            actualLongitude.Should().Be(expected?.ContactLocation?.Longitude);
        }

        private static void AssertCar(CarAd actual, CarAd expected)
        {
            actual.Should().NotBeNull();
            actual.Id.Should().Be(expected.Id);
            actual.UserId.Should().Be(expected.UserId);
            actual.CreatedAt.Should().Be(expected.CreatedAt);
            actual.UpdatedAt.Should().Be(expected.UpdatedAt);
            actual.State.Should().Be(expected.State);

            actual.Price.PriceInDollars.Should().Be(expected.Price.PriceInDollars);
            actual.ManufacturingYear.Year.Should().Be(expected.ManufacturingYear.Year);
            actual.Kilometers.Value.Should().Be(expected.Kilometers.Value);
            actual.Description.Value.Should().Be(expected.Description.Value);
            actual.ContactPhoneNumber.Value.Should().Be(expected.ContactPhoneNumber.Value);
            actual.ModelVersion.Value.Should().Be(expected.ModelVersion.Value);

            actual.Province.Id.Should().Be(expected.Province.Id);
            actual.BodyType.Id.Should().Be(expected.BodyType.Id);
            actual.Color.Id.Should().Be(expected.Color.Id);
            actual.FuelType.Id.Should().Be(expected.FuelType.Id);
            actual.GearboxType.Id.Should().Be(expected.GearboxType.Id);

            actual.ExteriorTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.ExteriorTypes.Select(r => r.Id).ToArray());
            actual.InsideTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.InsideTypes.Select(r => r.Id).ToArray());
            actual.SafetyTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.SafetyTypes.Select(r => r.Id).ToArray());
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
                                  .WithSafetyTypes(safetyTypes)
                                  .WithInsideTypes(insideTypes)
                                  .Build();
            await Given.AssumeCarAdInRepository(carInDb);
            return carInDb;
        }

        private static class ApiHelper
        {
            public static class Put
            {
                public static string UpdateCarAdUrl(Guid id)
                {
                    return $"/api/car-ads/{id}";
                }
            }
        }
        #endregion 
    }
}
