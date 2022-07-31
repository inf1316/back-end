using QvaCar.Api.Features.CarAds;
using QvaCar.Domain.CarAds;
using System;
using System.Linq;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    public class RegisterCarAdRequestBuilder
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
        private ExteriorType[] exteriorTypes = Array.Empty<ExteriorType>();
        private SafetyType[] safetyTypes = Array.Empty<SafetyType>();
        private InsideType[] insideTypes = Array.Empty<InsideType>();
        private RegisterCarAdCoordinate? contactLocation;

        public RegisterCarAdRequestBuilder WithPrice(long value)
        {
            this.price = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithProvinceId(int value)
        {
            this.provinceId = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithManufacturingYear(int value)
        {
            this.manufacturingYear = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithKilometers(int value)
        {
            this.kilometers = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithBodyTypeId(int value)
        {
            this.bodyTypeId = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithColorId(int value)
        {
            this.colorId = value;
            return this;
        }

        public RegisterCarAdRequestBuilder WithFuelTypeId(int value)
        {
            this.fuelTypeId = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithGearboxTypeId(int value)
        {
            this.gearboxTypeId = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithDescription(string value)
        {
            this.description = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithContactPhoneNumber(string value)
        {
            this.contactPhoneNumber = value;
            return this;
        }
        
        public RegisterCarAdRequestBuilder WithModelVersion(string value)
        {
            this.modelVersion = value;
            return this;
        }

        public RegisterCarAdRequestBuilder WithExteriorTypes(ExteriorType[] exteriorTypes) 
        {
            this.exteriorTypes = exteriorTypes;
            return this;
        }

        public RegisterCarAdRequestBuilder WithSafetyTypes(SafetyType[] safetyTypes)
        {
            this.safetyTypes = safetyTypes;
            return this;
        }

        public RegisterCarAdRequestBuilder WithInsideTypes(InsideType[] insideTypes)
        {
            this.insideTypes = insideTypes;
            return this;
        }

        public RegisterCarAdRequestBuilder WithContactLocation(RegisterCarAdCoordinate? contactLocation)
        {
            this.contactLocation = contactLocation;
            return this;
        }

        public RegisterCarAdRequest Build()
        {
            return new RegisterCarAdRequest()
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
                SafetyTypeIds = safetyTypes.Select(r =>r.Id).ToArray(),
            };
        }
    }
}
