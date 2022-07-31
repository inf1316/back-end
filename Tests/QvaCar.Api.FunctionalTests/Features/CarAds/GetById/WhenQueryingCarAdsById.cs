using FluentAssertions;
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
    public class WhenQueryingCarAdsById
    {
        private readonly TestServerFixture Given;
        private readonly TestApiUser ValidUser;
        private readonly ImageServiceOptions imageOptions;
        public WhenQueryingCarAdsById(TestServerFixture given)
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
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft);

            var requestUrl = ApiHelper.Get.GetAdByIdUrl(Guid.NewGuid());
            var response = await Given.Server.CreateClient().GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Id_Is_Not_Found()
        {
            var userId = ValidUser.Id;
            await GivenDefaultAdInRepositoryForUser(userId, AdState.Draft);

            var requestUrl = ApiHelper.Get.GetAdByIdUrl(Guid.NewGuid());
            var response = await Given.Server.CreateClient().FromUser(ValidUser).GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Element_When_There_Is_A_Single_Element_In_Db()
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
                                .WithExteriorTypes(exteriorTypes)
                                .WithInsideTypes(insideTypes)
                                .WithSafetyTypes(safetyTypes)
                                .WithContactLocation(contactLocation)
                                .Build();

            await Given.AssumeCarAdInRepository(carInDb);
            var response = await RequestAndGetAdById(ValidUser, carInDb.Id);

            response.Should().NotBeNull();
            AssertCarAdResponse(response, carInDb);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Ok_For_Single_Element_With_No_Location()
        {
            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, null, coordinate: null);
            
            var response = await RequestAndGetAdById(ValidUser, carInDb.Id);

            response.Should().NotBeNull();
            AssertCarAdResponse(response, carInDb);
            carInDb.ContactLocation.Should().BeNull();
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Element_With_Image_Single_Image()
        {
            var image = "myImage.jpg";
            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new[] { image });

            var response = await RequestAndGetAdById(ValidUser, carInDb.Id);

            response.Should().NotBeNull();
            response.Images.Should().NotBeNull().And.HaveCount(1);
            var imageResponse = response.Images.First();
            AssertImageResponse(imageResponse, image);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Element_With_Image_Multiple_Images()
        {
            var image = "myImage.jpg";
            var secondImage = "myImage2.jpg";
            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new[] { image, secondImage });

            var response = await RequestAndGetAdById(ValidUser, carInDb.Id);

            response.Should().NotBeNull();
            response.Images.Should().NotBeNull().And.HaveCount(2);

            var imageResponse = response.Images[0];
            var secondImageResponse = response.Images[1];

            AssertImageResponse(imageResponse, image);
            AssertImageResponse(secondImageResponse, secondImage);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Correct_Element_When_There_Are_More_Elements_In_Db()
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

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var carInDb = new CarAdBuilder()
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
                                .WithExteriorTypes(exteriorTypes)
                                .WithInsideTypes(insideTypes)
                                .WithSafetyTypes(safetyTypes)
                                .Build();
            await Given.AssumeCarAdInRepository(carInDb);

            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft);
            var response = await RequestAndGetAdById(ValidUser, carInDb.Id);

            response.Should().NotBeNull();
            AssertCarAdResponse(response, carInDb);
        }

        [Fact]
        [ResetApplicationState]
        public async void Does_Not_Returns_Other_Users_Ads()
        {
            var inValidUserId = Guid.NewGuid();
            await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft);
            var carInDbOfUser2 = await GivenDefaultAdInRepositoryForUser(inValidUserId, AdState.Draft);

            var requestUrl = ApiHelper.Get.GetAdByIdUrl(carInDbOfUser2.Id);
            var response = await Given.Server.CreateClient().FromUser(ValidUser).GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        [ResetApplicationState]
        public async void Does_Not_Returns_Unregistered_Ads()
        {
            var unregisteredAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Unregistered);
            var requestUrl = ApiHelper.Get.GetAdByIdUrl(unregisteredAd.Id);

            var response = await Given.Server.CreateClient().FromUser(ValidUser).GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Draft_Ads()
        {
            var draftAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft);

            var response = await RequestAndGetAdById(ValidUser, draftAd.Id);

            response.Should().NotBeNull();
            response.Id.Should().Be(draftAd.Id.ToString());
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Published_Ads()
        {
            var publishedAd = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Published);

            var response = await RequestAndGetAdById(ValidUser, publishedAd.Id);

            response.Should().NotBeNull();
            response.Id.Should().Be(publishedAd.Id.ToString());
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Element_With_Image_Single_Image_With_Proper_Url()
        {
            var image = "my Complex Image Name.jpg";
            var carInDb = await GivenDefaultAdInRepositoryForUser(ValidUser.Id, AdState.Draft, new[] { image });

            var response = await RequestAndGetAdById(ValidUser, carInDb.Id);

            response.Should().NotBeNull();
            response.Images.Should().NotBeNull().And.HaveCount(1);
            var imageResponse = response.Images.First();
            AssertImageResponse(imageResponse, image);
            AssertImageUrlsWithScaping(imageResponse, carInDb.UserId, carInDb.Id, image);
        }

        #region Helpers
        private async Task<CarAd> GivenDefaultAdInRepositoryForUser(Guid userId, AdState state, string[]? images = null, Coordinate? coordinate = null)
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
            var time = new DateTime(2020, 01, 01, 12, 20, 01);
            var updateTime = new DateTime(2020, 01, 01, 12, 20, 01);

            var exteriorTypes = new ExteriorType[] { ExteriorType.SportTires, ExteriorType.SolarRoof };
            var insideTypes = new InsideType[] { InsideType.AirConditioning, InsideType.ExternalViewMirrors };
            var safetyTypes = new SafetyType[] { SafetyType.Airbags, SafetyType.Alarma };

            var carInDb = new CarAdBuilder()
                                .WithAdState(state)
                                .WithCreatedAt(time)
                                .WithUpdateAt(updateTime)
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
                                .WithInsideTypes(insideTypes)
                                .WithSafetyTypes(safetyTypes)
                                .WithImages(images ?? Array.Empty<string>())
                                .WithContactLocation(coordinate)
                                .Build();
            await Given.AssumeCarAdInRepository(carInDb);
            return carInDb;
        }

        private async Task<CarAdByIdResponse> RequestAndGetAdById(TestApiUser asUser, Guid guid)
        {
            var requestUrl = ApiHelper.Get.GetAdByIdUrl(guid);
            var response = await Given.Server.CreateClient().FromUser(asUser).GetAsync(requestUrl);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = await response.Deserialize<CarAdByIdResponse>();
            return responseModel;
        }

        private static void AssertCarAdResponse(CarAdByIdResponse actual, CarAd expected)
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

            actual.ExteriorTypes.Select(r => r.Id).Should().BeEquivalentTo(expected.ExteriorTypes.Select(r => r.Id).ToArray());
            actual.ExteriorTypes.Select(r => r.Name).Should().BeEquivalentTo(expected.ExteriorTypes.Select(r => r.Name).ToArray());

            actual.SafetyTypes.Select(r => r.Id).Should().BeEquivalentTo(expected.SafetyTypes.Select(r => r.Id).ToArray());
            actual.SafetyTypes.Select(r => r.Name).Should().BeEquivalentTo(expected.SafetyTypes.Select(r => r.Name).ToArray());

            actual.InsideTypes.Select(r => r.Id).Should().BeEquivalentTo(expected.InsideTypes.Select(r => r.Id).ToArray());
            actual.InsideTypes.Select(r => r.Name).Should().BeEquivalentTo(expected.InsideTypes.Select(r => r.Name).ToArray());

            var actualLatitude = actual.ContactLocation?.Latitude;
            var actualLongitude = actual.ContactLocation?.Longitude;
            actualLatitude.Should().Be(expected?.ContactLocation?.Latitude);
            actualLongitude.Should().Be(expected?.ContactLocation?.Longitude);
        }

        private static void AssertImageResponse(CarAdImagebByIdResponse imageResponse, string expectedFileName)
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

        private void AssertImageUrlsWithScaping(CarAdImagebByIdResponse imageResponse, Guid userId, Guid adId, string expectedFileName)
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
                public static string GetAdByIdUrl(Guid id)
                {
                    return $"/api/car-ads/{id}";
                }
            }
        }
        #endregion 
    }
}
