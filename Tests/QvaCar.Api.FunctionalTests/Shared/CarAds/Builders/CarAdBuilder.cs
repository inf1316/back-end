using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;

namespace QvaCar.Api.FunctionalTests.Shared.CarAds
{
#nullable disable
    public class CarAdBuilder
    {
        private Guid userId;
        private DateTime createdAt;
        private DateTime updatedAt;
        private AdState state;
        private long price;
        private Province province;
        private int manufacturingYear;
        private int kilometers;
        private CarBodyType bodyType;
        private Color color;
        private FuelType fuelType;
        private GearboxType gearboxType;
        private string description = string.Empty;
        public string contactPhoneNumber;
        public string modelVersion;
        public Coordinate contactLocation;
        public string[] images;
        private IList<ExteriorType> exteriorTypes;
        private IList<InsideType> insideTypes;
        private IList<SafetyType> safetyTypes;       

        public CarAdBuilder WithUserId(Guid uId)
        {
            userId = uId;
            return this;
        }
        public CarAdBuilder WithCreatedAt(DateTime cDate)
        {
            createdAt = cDate;
            return this;
        }
        public CarAdBuilder WithUpdateAt(DateTime updateDate)
        {
            updatedAt = updateDate;
            return this;
        }
        public CarAdBuilder WithAdState(AdState adState)
        {
            state = adState;
            return this;
        }
        public CarAdBuilder WithPrice(long p)
        {
            price = p;
            return this;
        }
        public CarAdBuilder WithProvince(Province prov)
        {
            province = prov;
            return this;
        }
        public CarAdBuilder WithManufacturingYear(int mYear)
        {
            manufacturingYear = mYear;
            return this;
        }
        public CarAdBuilder WithKilometers(int kms)
        {
            kilometers = kms;
            return this;
        }
        public CarAdBuilder WithBodyType(CarBodyType bType)
        {
            bodyType = bType;
            return this;
        }
        public CarAdBuilder WithColor(Color c)
        {
            color = c;
            return this;
        }
        public CarAdBuilder WithFuelType(FuelType f)
        {
            fuelType = f;
            return this;
        }
        public CarAdBuilder WithDescription(string desc)
        {
            description = desc;
            return this;
        }
        public CarAdBuilder WithGearboxType(GearboxType gearbox)
        {
            gearboxType = gearbox;
            return this;
        }
        public CarAdBuilder WithContactPhoneNumber(string phoneNumber)
        {
            contactPhoneNumber = phoneNumber;
            return this;
        }
        public CarAdBuilder WithModelVersion(string version)
        {
            modelVersion = version;
            return this;
        }
        public CarAdBuilder WithImages(string[] images)
        {
            this.images = images;
            return this;
        }
        public CarAdBuilder WithContactLocation(Coordinate contactLocation)
        {
            this.contactLocation = contactLocation;
            return this;
        }
        public CarAdBuilder WithExteriorTypes(ExteriorType[] exteriorTypes)
        {
            this.exteriorTypes = exteriorTypes;                
            return this;
        }
        public CarAdBuilder WithSafetyTypes(SafetyType[] safetyTypes)
        {
            this.safetyTypes = safetyTypes;                
            return this;
        }
        public CarAdBuilder WithInsideTypes(InsideType[] insideTypes)
        {
            this.insideTypes = insideTypes;                
            return this;
        }

        public CarAd Build()
        {
            Check.IsNotNull<NullReferenceException>(state, $"{nameof(CarAd)}.{nameof(CarAd.State)} is required in the domain.");
            var ad = new CarAd
                (
                    userId,
                    createdAt,
                    price,
                    province,
                    manufacturingYear,
                    kilometers,
                    bodyType,
                    color,
                    fuelType,
                    gearboxType,
                    description,
                    contactPhoneNumber,                    
                    modelVersion,
                    contactLocation,
                    exteriorTypes,
                    safetyTypes,
                    insideTypes);

            SetPrivateProperty(ad, nameof(ad.UpdatedAt), updatedAt);
            SetPrivateProperty(ad, nameof(ad.State), state);

            if (images?.Length > 0)
                ad.AddImages(images);

            return ad;
        }

        private static void SetPrivateProperty(object instance, string propertyName, object value)
        {
            typeof(CarAd).GetProperty(propertyName)?.SetValue(instance, value);
        }      
    }
#nullable enable
}
