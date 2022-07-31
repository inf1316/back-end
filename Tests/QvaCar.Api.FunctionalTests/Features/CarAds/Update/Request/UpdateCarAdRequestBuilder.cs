using QvaCar.Api.Features.CarAds;
using QvaCar.Domain.CarAds;
using System;
using System.Linq;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    public class UpdateCarAdRequestBuilder
    {
        private long price;
        private int provinceId;
        private int manufacturingYear;
        private int kilometers;
        private int bodyTypeId;
        private int colorId;
        private int fuelTypeId;
        private int gearboxTypeId;
        private string description = string.Empty;
        private string contactPhoneNumber = string.Empty;
        private string modelVersion = string.Empty;
        private UpdateCarAdCoordinate? contactLocation;
        private ExteriorType[] exteriorTypes = Array.Empty<ExteriorType>();
        private SafetyType[] safetyTypes = Array.Empty<SafetyType>();
        private InsideType[] insideTypes = Array.Empty<InsideType>();

        public UpdateCarAdRequestBuilder WithPrice(long value)
        {
            this.price = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithProvinceId(int value)
        {
            this.provinceId = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithManufacturingYear(int value)
        {
            this.manufacturingYear = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithKilometers(int value)
        {
            this.kilometers = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithBodyTypeId(int value)
        {
            this.bodyTypeId = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithColorId(int value)
        {
            this.colorId = value;
            return this;
        }

        public UpdateCarAdRequestBuilder WithFuelTypeId(int value)
        {
            this.fuelTypeId = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithGearboxTypeId(int value)
        {
            this.gearboxTypeId = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithDescription(string value)
        {
            this.description = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithContactPhoneNumber(string value)
        {
            this.contactPhoneNumber = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithModelVersion(string value)
        {
            this.modelVersion = value;
            return this;
        }
        public UpdateCarAdRequestBuilder WithExteriorTypes(ExteriorType[] exteriorTypes)
        {
            this.exteriorTypes = exteriorTypes;
            return this;
        }
        public UpdateCarAdRequestBuilder WithSafetyTypes(SafetyType[] safetyTypes)
        {
            this.safetyTypes = safetyTypes;
            return this;
        }
        public UpdateCarAdRequestBuilder WithInsideTypes(InsideType[] insideTypes)
        {
            this.insideTypes = insideTypes;
            return this;
        }

        public UpdateCarAdRequestBuilder WithContactLocation(UpdateCarAdCoordinate? contactLocation)
        {
            this.contactLocation = contactLocation;
            return this;
        }

        public UpdateCarAdRequest Build()
        {
            return new UpdateCarAdRequest()
            {
                Price = price,
                ProvinceId = provinceId,
                ManufacturingYear = manufacturingYear,
                Kilometers = kilometers,
                BodyTypeId = bodyTypeId,
                ColorId = colorId,
                FuelTypeId = fuelTypeId,
                GearboxTypeId = gearboxTypeId,
                Description = description,
                ContactPhoneNumber = contactPhoneNumber,
                ModelVersion = modelVersion,
                ContactLocation = contactLocation,
                ExteriorTypeIds = exteriorTypes.Select(r => r.Id).ToArray(),
                InsideTypeIds = insideTypes.Select(r => r.Id).ToArray(),
                SafetyTypeIds = safetyTypes.Select(r => r.Id).ToArray()
            };
        }
    }
}
