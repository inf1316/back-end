using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Api.Features.CarAds;
using QvaCar.Api.FunctionalTests.SeedWork;
using QvaCar.Api.FunctionalTests.Shared.CarAds;
using QvaCar.Api.FunctionalTests.Shared.CarAds.Extensions;
using QvaCar.Api.FunctionalTests.Shared.Identity;
using QvaCar.Domain.CarAds;
using QvaCar.Infraestructure.BlogStorage.Configuration.Options;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    [Collection(nameof(TestServerFixtureCollection))]
    public class WhenQueryingAllCarAdsByUser
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;
        private readonly ImageServiceOptions imageOptions;
        public WhenQueryingAllCarAdsByUser(TestServerFixture given)
        {
            Given = given ?? throw new Exception("Null Server");
            ValidUser = new TestApiUserBuilder()
               .WithId(Guid.NewGuid())
               .WithFreeSubscriptionLevel()
               .Build();
            imageOptions = Given.Server.Services.GetRequiredService<ImageServiceOptions>();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Unauthorized_When_User_Is_Not_Logged_In()
        {
            var requestUrl = ApiHelper.Get.GetAllCarsAdsUrl();

            var response = await Given.Server.CreateClient().GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var responseModel = await response.Deserialize<ProblemDetails>();
            responseModel.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Empty_When_Db_Is_Empty()
        {
            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Ok_For_Single_Element()
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
            var createdAt = new DateTime(2020, 01, 01, 12, 20, 01);
            var updateTime = new DateTime(2020, 01, 01, 12, 20, 01);
            var state = AdState.Draft;
            var contactLocation = Coordinate.FromLatLon(20, 25);
            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };


            var carInDb = new CarAdBuilder()
                                .WithAdState(state)
                                .WithCreatedAt(createdAt)
                                .WithUpdateAt(updateTime)
                                .WithUserId(ValidUser.Id)
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
                                .WithAdState(AdState.Draft)
                                .WithContactLocation(contactLocation)
                                .WithExteriorTypes(exteriorTypes)
                                .WithInsideTypes(insideTypes)
                                .WithSafetyTypes(safetyTypes)
                                .Build();
            await Given.AssumeCarAdInRepository(carInDb);

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(1);

            var responseCar = response.Ads.First();
            AssertCarAdResponse(responseCar, carInDb);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Ok_For_Single_Element_With_Image()
        {
            var image = "myImage.jpg";
            var createdAt = new DateTime(2020, 01, 01, 12, 20, 01);
            var state = AdState.Draft;
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt, new string[] { image });

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(1);

            var responseItem = response.Ads.First();

            responseItem.Images.Should().NotBeNull().And.HaveCount(1);
            var imageResponse = responseItem.Images.First();
            AssertImageResponse(imageResponse, image);
        }

        [Fact]
        [ResetApplicationState]
        public async void Works_For_Multiple_Elements()
        {
            var car1PriceInDollars = 1000;
            var car1Province = Province.Cienfuegos;
            var car1ManufacturingYear = 1990;
            var car1Kilometers = 100;
            var car1BodyType = CarBodyType.Berlina;
            var car1Color = Color.Blanco;
            var car1FuelType = FuelType.Diesel;
            var car1GearboxType = GearboxType.Manual;
            var car1Description = "Some description";
            var car1ContactPhoneNumber = "551011";
            var car1Version = "Golf GTI";

            var car1ExteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var car1InsideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var car1SafetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var car2PriceInDollars = 1500;
            var car2Province = Province.SantiagoDeCuba;
            var car2ManufacturingYear = 1991;
            var car2Kilometers = 101;
            var car2BodyType = CarBodyType.Familiar;
            var car2Color = Color.Negro;
            var car2FuelType = FuelType.Gasolina;
            var car2GearboxType = GearboxType.Automatico;
            var car2Description = "Some description 2";
            var car2ContactPhoneNumber = "551012";
            var car2Version = "Yaris";

            var car2ExteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var car2InsideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var car2SafetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var car1InDb = new CarAdBuilder()
                                  .WithUserId(ValidUser.Id)
                                  .WithPrice(car1PriceInDollars)
                                  .WithProvince(car1Province)
                                  .WithManufacturingYear(car1ManufacturingYear)
                                  .WithKilometers(car1Kilometers)
                                  .WithBodyType(car1BodyType)
                                  .WithColor(car1Color)
                                  .WithFuelType(car1FuelType)
                                  .WithGearboxType(car1GearboxType)
                                  .WithDescription(car1Description)
                                  .WithContactPhoneNumber(car1ContactPhoneNumber)
                                  .WithModelVersion(car1Version)
                                  .WithAdState(AdState.Draft)
                                  .WithExteriorTypes(car1ExteriorTypes)
                                  .WithInsideTypes(car1InsideTypes)
                                  .WithSafetyTypes(car1SafetyTypes)
                                  .Build();
            await Given.AssumeCarAdInRepository(car1InDb);

            var car2InDb = new CarAdBuilder()
                                  .WithUserId(ValidUser.Id)
                                  .WithPrice(car2PriceInDollars)
                                  .WithProvince(car2Province)
                                  .WithManufacturingYear(car2ManufacturingYear)
                                  .WithKilometers(car2Kilometers)
                                  .WithBodyType(car2BodyType)
                                  .WithColor(car2Color)
                                  .WithFuelType(car2FuelType)
                                  .WithGearboxType(car2GearboxType)
                                  .WithDescription(car2Description)
                                  .WithContactPhoneNumber(car2ContactPhoneNumber)
                                  .WithModelVersion(car2Version)
                                  .WithAdState(AdState.Draft)
                                  .WithExteriorTypes(car2ExteriorTypes)
                                  .WithInsideTypes(car2InsideTypes)
                                  .WithSafetyTypes(car2SafetyTypes)
                                  .Build();
            await Given.AssumeCarAdInRepository(car2InDb);

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(2);

            var responseCar1 = response.Ads.Single(x => x.Id == car1InDb.Id.ToString());
            var responseCar2 = response.Ads.Single(x => x.Id == car2InDb.Id.ToString());

            AssertCarAdResponse(responseCar1, car1InDb);
            AssertCarAdResponse(responseCar2, car2InDb);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Ok_For_Single_Element_With_No_Location()
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
            var createdAt = new DateTime(2020, 01, 01, 12, 20, 01);
            var updateTime = new DateTime(2020, 01, 01, 12, 20, 01);
            var state = AdState.Draft;
            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };


            var carInDb = new CarAdBuilder()
                                .WithAdState(state)
                                .WithCreatedAt(createdAt)
                                .WithUpdateAt(updateTime)
                                .WithUserId(ValidUser.Id)
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
                                .WithAdState(AdState.Draft)
                                .WithContactLocation(null)
                                .WithExteriorTypes(exteriorTypes)
                                .WithInsideTypes(insideTypes)
                                .WithSafetyTypes(safetyTypes)
                                .Build();
            await Given.AssumeCarAdInRepository(carInDb);

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(1);

            var responseCar = response.Ads.First();
            AssertCarAdResponse(responseCar, carInDb);
            carInDb.ContactLocation.Should().BeNull();
        }


        [Fact]
        [ResetApplicationState]
        public async void Returns_Ok_For_Multiple_Elements_With_Images()
        {
            var image = "myImage.jpg";
            var secondElementImage = "myImage2.jpg";
            var createdAt = new DateTime(2020, 01, 01, 12, 20, 01);
            var state = AdState.Draft;

            var firtsAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt, new string[] { image });
            var secondAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt, new string[] { secondElementImage });

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(2);

            var responseItem = response.Ads.First();
            responseItem.Id.Should().Be(firtsAd.Id.ToString());
            responseItem.Images.Should().NotBeNull().And.HaveCount(1);
            var firstItemImageResponse = responseItem.Images.First();

            var secondAdResponse = response.Ads.ToList()[1];
            secondAdResponse.Id.Should().Be(secondAd.Id.ToString());
            secondAdResponse.Images.Should().NotBeNull().And.HaveCount(1);
            var secondImageResponse = secondAdResponse.Images.First();


            AssertImageResponse(firstItemImageResponse, image);
            AssertImageResponse(secondImageResponse, secondElementImage);
        }

        [Fact]
        [ResetApplicationState]
        public async void Get_All_Does_Not_Returns_Other_Users_Ads()
        {
            var invalidUserId = Guid.NewGuid();

            var car1InDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));
            var car2InDb = await GivenDefaultAdInRepositoryForUser(invalidUserId, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(1);

            var responseCar1 = response.Ads.Single(x => x.Id == car1InDb.Id.ToString());

            AssertCarAdResponse(responseCar1, car1InDb);
        }

        [Fact]
        [ResetApplicationState]
        public async void Get_Does_Not_Returns_Unregistered_Ads()
        {
            var draftAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new DateTime(2021, 01, 02, 03, 04, 05));
            var publisheAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Published, new DateTime(2021, 01, 02, 03, 04, 05));
            var unregisteredAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Unregistered, new DateTime(2021, 01, 02, 03, 04, 05));

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(2);

            var draftAdResponse = response.Ads.FirstOrDefault(x => x.Id == draftAd.Id.ToString());
            var publisheAdResponse = response.Ads.FirstOrDefault(x => x.Id == publisheAd.Id.ToString());

            draftAdResponse.Should().NotBeNull();
            publisheAdResponse.Should().NotBeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Elements_Sorted_By_Creation_Date()
        {
            var secondCreationDate = new DateTime(2021, 01, 02, 03, 04, 05);
            var firstCreationDate = new DateTime(2021, 01, 02, 03, 04, 05).AddDays(-1);
            var lastCreationDate = new DateTime(2021, 01, 02, 03, 04, 05).AddDays(1);

            var secondAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, secondCreationDate);
            var lastAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, lastCreationDate);
            var firstAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, firstCreationDate);

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(3);
            var ads = response.Ads.ToList();

            ads[0].Id.Should().Be(firstAd.Id.ToString());
            ads[1].Id.Should().Be(secondAd.Id.ToString());
            ads[2].Id.Should().Be(lastAd.Id.ToString());
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Ok_For_Single_Element_With_Image_WithProper_Url()
        {
            var image = "my Image.jpg";
            var createdAt = new DateTime(2020, 01, 01, 12, 20, 01);
            var state = AdState.Draft;
            var modelInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, state, createdAt, new string[] { image });

            var response = await RequestAndGetAllAdsByUserAsync(ValidUser);

            response.Should().NotBeNull();
            response.Ads.Should().NotBeNull().And.HaveCount(1);

            var responseItem = response.Ads.First();

            responseItem.Images.Should().NotBeNull().And.HaveCount(1);
            var imageResponse = responseItem.Images.First();
            AssertImageResponse(imageResponse, image);
            AssertImageUrlsWithScaping(imageResponse, ValidUser.Id, modelInDb.Id, image);
        }

        #region Helpers
        private async Task<CarAd> GivenDefaultAdInRepositoryForUser(Guid userId, AdState state, DateTime createdAt, string[]? images = null)
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
                                   .WithImages(images ?? Array.Empty<string>())
                                   .WithExteriorTypes(exteriorTypes)
                                   .WithInsideTypes(insideTypes)
                                   .WithSafetyTypes(safetyTypes)
                                   .Build();
            await Given.AssumeCarAdInRepository(carInDb);
            return carInDb;
        }

        private async Task<CarAdsResponse> RequestAndGetAllAdsByUserAsync(TestApiUser asUser)
        {
            var requestUrl = ApiHelper.Get.GetAllCarsAdsUrl();
            var response = await Given.Server.CreateClient().FromUser(asUser).GetAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = await response.Deserialize<CarAdsResponse>();
            return responseModel;
        }
        private static void AssertCarAdResponse(CarAdListItemResponse actual, CarAd expected)
        {
            actual.Should().NotBeNull();
            actual.Id.Should().Be(expected.Id.ToString());
            actual.UserId.Should().Be(expected.UserId);

            actual.CreatedAt.Should().Be(expected.CreatedAt);
            actual.UpdatedAt.Should().Be(expected.UpdatedAt);

            actual.StateId.Should().Be(expected.State.Id);
            actual.StateName.Should().Be(expected.State.Name);

            actual.Price.Should().Be(expected.Price.PriceInDollars);
            actual.ManufacturingYear.Should().Be(expected.ManufacturingYear.Year);
            actual.Kilometers.Should().Be(expected.Kilometers.Value);
            actual.Description.Should().Be(expected.Description.Value);
            actual.ContactPhoneNumber.Should().Be(expected.ContactPhoneNumber.Value);
            actual.ModelVersion.Should().Be(expected.ModelVersion.Value);

            actual.ProvinceId.Should().Be(expected.Province.Id);
            actual.ProvinceName.Should().Be(expected.Province.Name);

            actual.BodyTypeId.Should().Be(expected.BodyType.Id);
            actual.BodyTypeName.Should().Be(expected.BodyType.Name);

            actual.ColorId.Should().Be(expected.Color.Id);
            actual.ColorName.Should().Be(expected.Color.Name);

            actual.FuelTypeId.Should().Be(expected.FuelType.Id);
            actual.FuelTypeName.Should().Be(expected.FuelType.Name);

            actual.GearboxTypeId.Should().Be(expected.GearboxType.Id);
            actual.GearboxTypeName.Should().Be(expected.GearboxType.Name);

            var actualLatitude = actual.ContactLocation?.Latitude;
            var actualLongitude = actual.ContactLocation?.Longitude;
            actualLatitude.Should().Be(expected?.ContactLocation?.Latitude);
            actualLongitude.Should().Be(expected?.ContactLocation?.Longitude);

            actual.ExteriorTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.ExteriorTypes.Select(r => r.Id).ToArray());
            actual.ExteriorTypes.Select(r => r.Name).ToArray().Should().BeEquivalentTo(expected.ExteriorTypes.Select(r => r.Name).ToArray());

            actual.SafetyTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.SafetyTypes.Select(r => r.Id).ToArray());
            actual.SafetyTypes.Select(r => r.Name).ToArray().Should().BeEquivalentTo(expected.SafetyTypes.Select(r => r.Name).ToArray());

            actual.InsideTypes.Select(r => r.Id).ToArray().Should().BeEquivalentTo(expected.InsideTypes.Select(r => r.Id).ToArray());
            actual.InsideTypes.Select(r => r.Name).ToArray().Should().BeEquivalentTo(expected.InsideTypes.Select(r => r.Name).ToArray());
        }

        private static void AssertImageResponse(CarAdListImageResponse imageResponse, string expectedFileName)
        {
            var expectedSizes = new string[] { "small", "medium", "large" };
            imageResponse.FileName.Should().Be(expectedFileName);
            imageResponse.Urls.Should().NotBeNull().And.HaveCount(3);

            foreach (var img in imageResponse.Urls)
            {
                img.Url.Should().NotBeNull();
                img.Url.ToString().Should().NotBeEmpty();
                img.Size.Should().NotBeNull().And.NotBeEmpty();
            }
            var allSizes = imageResponse.Urls.Select(x => x.Size);
            allSizes.Should().BeEquivalentTo(expectedSizes);
        }

        private void AssertImageUrlsWithScaping(CarAdListImageResponse imageResponse, Guid userId, Guid adId, string expectedFileName)
        {
            imageResponse.FileName.Should().Be(expectedFileName);
            imageResponse.Urls.Should().NotBeNull().And.HaveCount(3);

            foreach (var img in imageResponse.Urls)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(expectedFileName);
                string extension = Path.GetExtension(expectedFileName);
                img.Url.Should().NotBeNull();
                img.Url.ToString().Should().NotBeEmpty();
                img.Size.Should().NotBeNull().And.NotBeEmpty();
                string expectedUrl = $"{ imageOptions.BaseUrl}{imageOptions.ContainerName}/{userId}/{adId}/{System.Web.HttpUtility.UrlPathEncode(fileNameWithoutExtension)}_{img.Size}{extension}";
                img.Url.AbsoluteUri.Should().Be(expectedUrl);
            }
        }

        private static class ApiHelper
        {
            public static class Get
            {
                public static string GetAllCarsAdsUrl()
                {
                    return "/api/car-ads/";
                }
            }
        }
        #endregion 
    }
}
