using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Domain.CarAds
{
    public class CarAd : AggregateRoot<Guid>
    {
        public Guid UserId { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }
        public AdState State { get; private set; }
        public Price Price { get; private set; }
        public Province Province { get; private set; }
        public ManufacturingYear ManufacturingYear { get; private set; }
        public Kilometers Kilometers { get; private set; }
        public CarBodyType BodyType { get; private set; }
        public Color Color { get; private set; }
        public FuelType FuelType { get; private set; }
        public GearboxType GearboxType { get; private set; }
        public CarAdDescription Description { get; private set; }
        public CarAdContactPhoneNumber ContactPhoneNumber { get; private set; }
        public ModelVersion ModelVersion { get; private set; }
        public Coordinate? ContactLocation { get; private set; }
        public IReadOnlyCollection<SafetyType> SafetyTypes => safetyTypes.ToList();
        private List<SafetyType> safetyTypes = new();
        public IReadOnlyCollection<InsideType> InsideTypes => insideTypes.ToList();
        private List<InsideType> insideTypes = new();
        public IReadOnlyCollection<ExteriorType> ExteriorTypes => exteriorTypes.ToList();
        private List<ExteriorType> exteriorTypes = new();
        public IReadOnlyList<Image> Images => images.ToList();
        private List<Image> images = new();

#nullable disable
        private CarAd() { }
#nullable enable

        public CarAd
            (
                Guid userId,
                DateTime creationDate,
                long priceInDollars,
                Province province,
                int manufacturingYear,
                int kilometers,
                CarBodyType bodyType,
                Color color,
                FuelType fuelType,
                GearboxType gearboxType,
                string description,
                string contactPhoneNumber,
                string modelVersion,
                Coordinate? contactLocation,
                IList<ExteriorType> exteriorTypes,
                IList<SafetyType> safetyTypes,
                IList<InsideType> insideTypes
            )
        {
            Check.IsNotNull<DomainValidationException>(province, "Province is required.");
            Check.IsNotNull<DomainValidationException>(bodyType, "Body Type is required.");
            Check.IsNotNull<DomainValidationException>(color, "Color is required.");
            Check.IsNotNull<DomainValidationException>(fuelType, "Fuel type is required.");
            Check.IsNotNull<DomainValidationException>(gearboxType, "Gearbox type is required.");
            Check.IsNotNull<DomainValidationException>(exteriorTypes, "Exterior type is required.");
            Check.IsNotNull<DomainValidationException>(safetyTypes, "Safety type is required.");
            Check.IsNotNull<DomainValidationException>(insideTypes, "Inside type is required.");

            Id = Guid.NewGuid();
            State = AdState.Draft;
            CreatedAt = creationDate;
            UpdatedAt = creationDate;
            UserId = userId;
            Price = Price.FromDollars(priceInDollars);
            Province = province;
            Kilometers = Kilometers.FromKilometers(kilometers);
            ManufacturingYear = new ManufacturingYear(manufacturingYear);
            BodyType = bodyType;
            Color = color;
            FuelType = fuelType;
            GearboxType = gearboxType;
            Description = new CarAdDescription(description);
            ContactPhoneNumber = new CarAdContactPhoneNumber(contactPhoneNumber);
            ModelVersion = new ModelVersion(modelVersion);
            ContactLocation = contactLocation;
            this.exteriorTypes = exteriorTypes.ToList();
            this.insideTypes = insideTypes.ToList();
            this.safetyTypes = safetyTypes.ToList();
        }

        public void Update
            (
                DateTime updateDate,
                long priceInDollars,
                Province province,
                int manufacturingYear,
                int kilometers,
                CarBodyType bodyType,
                Color color,
                FuelType fuelType,
                GearboxType gearboxType,
                string description,
                string contactPhoneNumber,
                string modelVersion,
                Coordinate? contactLocation,
                IList<ExteriorType> exteriorTypes,
                IList<SafetyType> safetyTypes,
                IList<InsideType> insideTypes
            )
        {
            if (State == AdState.Unregistered)
                throw new DomainInvalidOperationException("Entity is unregistered and cannot be updated");

            Check.IsNotNull<DomainValidationException>(province, "Province is required.");
            Check.IsNotNull<DomainValidationException>(bodyType, "Body Type is required.");
            Check.IsNotNull<DomainValidationException>(color, "Color is required.");
            Check.IsNotNull<DomainValidationException>(fuelType, "Fuel type is required.");
            Check.IsNotNull<DomainValidationException>(gearboxType, "Gearbox type is required.");
            Check.IsNotNull<DomainValidationException>(exteriorTypes, "Exterior type is required.");
            Check.IsNotNull<DomainValidationException>(safetyTypes, "Safety type is required.");
            Check.IsNotNull<DomainValidationException>(insideTypes, "Inside type is required.");

            UpdatedAt = updateDate;
            Price = Price.FromDollars(priceInDollars);
            Province = province;
            Kilometers = Kilometers.FromKilometers(kilometers);
            ManufacturingYear = new ManufacturingYear(manufacturingYear);
            BodyType = bodyType;
            Color = color;
            FuelType = fuelType;
            GearboxType = gearboxType;
            Description = new CarAdDescription(description);
            ContactPhoneNumber = new CarAdContactPhoneNumber(contactPhoneNumber);
            ModelVersion = new ModelVersion(modelVersion);
            ContactLocation = contactLocation;
            this.exteriorTypes = exteriorTypes.ToList();
            this.insideTypes = insideTypes.ToList();
            this.safetyTypes = safetyTypes.ToList();
            AddDomainEvent(AdUpdatedDomainEvent.FromCarAd(this));
        }

        public void Unregister(DateTime updateDate)
        {
            if (State == AdState.Unregistered)
                throw new DomainInvalidOperationException("Entity is already unregistered");

            UpdatedAt = updateDate;
            State = AdState.Unregistered;

            AddDomainEvent(AdUnregisteredDomainEvent.FromCarAd(this));
        }

        public void Publish(DateTime updateDate)
        {
            if (State == AdState.Unregistered)
                throw new DomainInvalidOperationException("Entity is unregistered and cannot be published");
            if (State == AdState.Published)
                throw new DomainInvalidOperationException("Entity is already published.");

            UpdatedAt = updateDate;
            State = AdState.Published;

            AddDomainEvent(AdPublishedDomainEvent.FromCarAd(this));
        }

        public void AddImages(params string[] imageFileNamesWithExtension)
        {
            foreach (var fileName in imageFileNamesWithExtension)
            {
                if (images.Any(x => x.FileName == fileName))
                    throw new DomainInvalidOperationException($"Image '{fileName}' already exist.");
                var newImage = new Image(fileName);
                images.Add(newImage);
            }
            AddDomainEvent(ImageAddedToAdDomainEvent.FromImages(this.Id, this.State.Id, imageFileNamesWithExtension));
        }
    }
}
