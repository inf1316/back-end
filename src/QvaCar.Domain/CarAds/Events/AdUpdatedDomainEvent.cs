using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Domain.CarAds
{
    public record AdUpdatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public int StateId { get; init; }
        public string StateName { get; init; } = string.Empty;
        public Price Price { get; init; }
        public int ProvinceId { get; init; }
        public string ProvinceName { get; init; } = string.Empty;
        public ManufacturingYear ManufacturingYear { get; init; }
        public Kilometers Kilometers { get; init; }
        public int BodyTypeId { get; init; }
        public string BodyTypeName { get; init; } = string.Empty;
        public int ColorId { get; init; }
        public string ColorName { get; init; } = string.Empty;
        public int FuelTypeId { get; init; }
        public string FuelTypeName { get; init; } = string.Empty;
        public int GearboxTypeId { get; init; }
        public string GearboxTypeName { get; init; } = string.Empty;
        public CarAdDescription Description { get; init; }
        public CarAdContactPhoneNumber ContactPhoneNumber { get; init; }
        public ModelVersion ModelVersion { get; init; }
        public Coordinate? ContactLocation { get; init; }
        public IReadOnlyCollection<ExteriorTypeDto> ExteriorTypes { get; init; }
        public IReadOnlyCollection<InsideTypeDto> InsideTypes { get; init; }
        public IReadOnlyCollection<SafetyTypeDto> SafetyTypes { get; init; }

        public AdUpdatedDomainEvent
            (
                Guid id,
                Guid userId,
                DateTime createdAt,
                DateTime updatedAt,
                int stateId,
                string stateName,
                Price price,
                int provinceId,
                string provinceName,
                ManufacturingYear manufacturingYear,
                Kilometers kilometers,
                int bodyTypeId,
                string bodyTypeName,
                int colorId,
                string colorName,
                int fuelTypeId,
                string fuelTypeName,
                int gearboxTypeId,
                string gearboxTypeName,
                CarAdDescription description,
                CarAdContactPhoneNumber contactPhoneNumber,
                ModelVersion modelVersion,
                Coordinate? contactLocation,
                IReadOnlyCollection<ExteriorTypeDto> exteriorTypes,
                IReadOnlyCollection<InsideTypeDto> insideTypes,
                IReadOnlyCollection<SafetyTypeDto> safetyTypes
            )
        {
            Id = id;
            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            StateId = stateId;
            StateName = stateName;
            Price = price;
            ProvinceId = provinceId;
            ProvinceName = provinceName;
            ManufacturingYear = manufacturingYear;
            Kilometers = kilometers;
            BodyTypeId = bodyTypeId;
            BodyTypeName = bodyTypeName;
            ColorId = colorId;
            ColorName = colorName;
            FuelTypeId = fuelTypeId;
            FuelTypeName = fuelTypeName;
            GearboxTypeId = gearboxTypeId;
            GearboxTypeName = gearboxTypeName;
            Description = description;
            ContactPhoneNumber = contactPhoneNumber;
            ModelVersion = modelVersion;
            ExteriorTypes = exteriorTypes;
            InsideTypes = insideTypes;
            SafetyTypes = safetyTypes;
            ContactLocation = contactLocation;
        }

        public static AdUpdatedDomainEvent FromCarAd(CarAd ad)
        {
            var exteriorTypesDto = ad.ExteriorTypes.Select(x => ExteriorTypeDto.FromDomain(x)).ToList().AsReadOnly();
            var insideTypesDto = ad.InsideTypes.Select(x => InsideTypeDto.FromDomain(x)).ToList().AsReadOnly();
            var safetyTypesDto = ad.SafetyTypes.Select(x => SafetyTypeDto.FromDomain(x)).ToList().AsReadOnly();
            return new AdUpdatedDomainEvent
                (
                    ad.Id,
                    ad.UserId,
                    ad.CreatedAt,
                    ad.UpdatedAt,
                    ad.State.Id,
                    ad.State.Name,
                    ad.Price,
                    ad.Province.Id,
                    ad.Province.Name,
                    ad.ManufacturingYear,
                    ad.Kilometers,
                    ad.BodyType.Id,
                    ad.BodyType.Name,
                    ad.Color.Id,
                    ad.Color.Name,
                    ad.FuelType.Id,
                    ad.FuelType.Name,
                    ad.GearboxType.Id,
                    ad.GearboxType.Name,
                    ad.Description,
                    ad.ContactPhoneNumber,
                    ad.ModelVersion,
                    ad.ContactLocation,
                    exteriorTypesDto,
                    insideTypesDto,
                    safetyTypesDto
                );
        }

        public record ExteriorTypeDto
        {
            public int Id { get; init; }
            public string Name { get; init; }

            private ExteriorTypeDto(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public static ExteriorTypeDto FromDomain(ExteriorType exteriorType) => new(exteriorType.Id, exteriorType.Name);
        }

        public record SafetyTypeDto
        {
            public int Id { get; init; }
            public string Name { get; init; }

            private SafetyTypeDto(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public static SafetyTypeDto FromDomain(SafetyType safetyType) => new(safetyType.Id, safetyType.Name);
        }

        public record InsideTypeDto
        {
            public int Id { get; init; }
            public string Name { get; init; }
            private InsideTypeDto(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public static InsideTypeDto FromDomain(InsideType insideType) => new(insideType.Id, insideType.Name);
        }
    }
}