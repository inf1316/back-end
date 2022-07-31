using MediatR;
using QvaCar.Application.Services;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using QvaCar.Seedwork.Domain.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.CarAds
{
    public class RegisterCarAdCommandHandler : IRequestHandler<RegisterCarAdCommand, Guid>
    {
        private readonly ICarBodyTypeRepository _carBodyTypeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IFuelTypeRepository _fuelTypeRepository;
        private readonly IGearboxTypeRepository _gearboxTypeRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly ICarAdRepository _carAdRepository;
        private readonly IExteriorTypeRepository _exteriorTypeRepository;
        private readonly IInsidesTypesRepository _insidesTypesRepository;
        private readonly ISafetyTypeRepository _safetyTypeRepository;
        private readonly IClockService _clockService;
        private readonly ICurrentUserService _currentUserService;

        public RegisterCarAdCommandHandler
            (
                ICarBodyTypeRepository carBodyTypeRepository,
                IColorRepository colorRepository,
                IFuelTypeRepository fuelTypeRepository,
                IGearboxTypeRepository gearboxTypeRepository,
                IProvinceRepository provinceRepository,
                ICarAdRepository carAdRepository,
                IClockService clockService,
                IExteriorTypeRepository exteriorTypeRepository,
                IInsidesTypesRepository insidesTypesRepository,
                ISafetyTypeRepository safetyTypeRepository,
                ICurrentUserService currentUserService
            )
        {
            _carBodyTypeRepository = carBodyTypeRepository;
            _exteriorTypeRepository = exteriorTypeRepository;
            _safetyTypeRepository = safetyTypeRepository;
            _insidesTypesRepository = insidesTypesRepository;
            _colorRepository = colorRepository;
            _fuelTypeRepository = fuelTypeRepository;
            _gearboxTypeRepository = gearboxTypeRepository;
            _provinceRepository = provinceRepository;
            _carAdRepository = carAdRepository;
            _clockService = clockService;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(RegisterCarAdCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetCurrentUser().Id;

            var bodyType = _carBodyTypeRepository.GetByIdOrDefault(command.BodyTypeId);
            var color = _colorRepository.GetByIdOrDefault(command.ColorId);
            var fuelType = _fuelTypeRepository.GetByIdOrDefault(command.FuelTypeId);
            var gearBoxType = _gearboxTypeRepository.GetByIdOrDefault(command.GearboxTypeId);
            var province = _provinceRepository.GetByIdOrDefault(command.ProvinceId);
            var location = command.ContactLocation is not null ? Coordinate.FromLatLon(command.ContactLocation.Latitude, command.ContactLocation.Longitude) : null;

            var exteriorTypes = command.ExteriorTypeIds.Select(r => _exteriorTypeRepository.GetByIdOrDefault(r)).ToList();
            var safetyTypes = command.SafetyTypeIds.Select(r => _safetyTypeRepository.GetByIdOrDefault(r)).ToList();
            var insideTypes = command.InsideTypeIds.Select(r => _insidesTypesRepository.GetByIdOrDefault(r)).ToList();

            Validate.IsNotNull(bodyType, nameof(command.BodyTypeId), "Invalid car body type.");
            Validate.IsNotNull(color, nameof(command.ColorId), "Invalid color.");
            Validate.IsNotNull(fuelType, nameof(command.FuelTypeId), "Invalid fuel type.");
            Validate.IsNotNull(gearBoxType, nameof(command.GearboxTypeId), "Invalid gearbox type.");
            Validate.IsNotNull(province, nameof(command.ProvinceId), "Invalid province.");

#nullable disable
            var registerAd = new CarAd(
                    userId, _clockService.Now, command.Price, province,
                    command.ManufacturingYear, command.Kilometers,
                    bodyType, color, fuelType,
                    gearBoxType, command.Description, command.ContactPhoneNumber,
                    command.ModelVersion,
                    location,
                    exteriorTypes,
                    safetyTypes,
                    insideTypes
            );
#nullable disable

            await _carAdRepository.AddAsync(registerAd, cancellationToken);

            return registerAd.Id;
        }
    }
}
