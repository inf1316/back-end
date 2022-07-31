using FluentAssertions;
using QvaCar.Api.Features.CarAds;
using QvaCar.Api.FunctionalTests.SeedWork;
using QvaCar.Api.FunctionalTests.Shared.CarAds.Extensions;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    [Collection(nameof(TestServerFixtureCollection))]
    public class WhenQueryingReferenceData
    {
        private readonly TestServerFixture Given;

        public WhenQueryingReferenceData(TestServerFixture given)
        {
            Given = given ?? throw new Exception("Null Server");
        }

        [Fact]
        [ResetApplicationState]
        public async void Returns_Not_Found_When_Id_Is_Not_Found()
        {
            var expectedProvinces = Given.GetAllProvincesReferenceData();
            var expectedCarBodyTypes = Given.GetAllCarBodyTypesReferenceData();
            var expectedColors = Given.GetAllColorReferenceData();
            var expectedFuelTypes = Given.GetFuelTypesReferenceData();
            var expectedGearboxTypes = Given.GetGearboxTypesReferenceData();
            var expectedAdStates = Given.GetAllAdStateReferenceData();
            var expectedExteriorTypes = Given.GetExteriorTypesReferenceData();
            var expectedSafetyTypes = Given.GetSafetyTypesReferenceData();
            var expectedInsideTypes = Given.GetInsideTypesReferenceData();

            var response = await RequestAndGetReferenceData();

            response.Should().NotBeNull();
            AssertResponseField(response.States.ToList(), expectedAdStates.ToList());
            AssertResponseField(response.Provinces.ToList(), expectedProvinces.ToList());
            AssertResponseField(response.CarBodyTypes.ToList(), expectedCarBodyTypes.ToList());
            AssertResponseField(response.Colors.ToList(), expectedColors.ToList());
            AssertResponseField(response.FuelTypes.ToList(), expectedFuelTypes.ToList());
            AssertResponseField(response.GearboxTypes.ToList(), expectedGearboxTypes.ToList());            
            AssertResponseField(response.ExteriorTypes.ToList(), expectedExteriorTypes.ToList());            
            AssertResponseField(response.SafetyTypes.ToList(), expectedSafetyTypes.ToList());
            AssertResponseField(response.InsideTypes.ToList(), expectedInsideTypes.ToList());            
        }

        #region Helpers
        private async Task<CarAdReferenceDataResponse> RequestAndGetReferenceData()
        {
            var requestUrl = ApiHelper.Get.ReferenceDataUrl();
            var response = await Given.Server.CreateRequest(requestUrl).GetAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = await response.Deserialize<CarAdReferenceDataResponse>();
            return responseModel;
        }

        private static void AssertResponseField(IReadOnlyList<BaseReferenceDataItemResponse> referenceDataField, IReadOnlyList<Enumeration> expectedReferenceDataFieldValues)
        {
            referenceDataField.Should().NotBeNull().And.HaveCount(expectedReferenceDataFieldValues.Count);

            foreach (var actual in referenceDataField)
            {
                var expected = expectedReferenceDataFieldValues.Single(x => x.Id == actual.Id);
                actual.Id.Should().Be(expected.Id);
                actual.Name.Should().Be(expected.Name);
            }
        }

        private static class ApiHelper
        {
            public static class Get
            {
                public static string ReferenceDataUrl()
                {
                    return $"/api/car-ads/reference-data";
                }
            }
        }
        #endregion 
    }
}
