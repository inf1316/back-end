using MediatR;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.CarAds
{
    public class GetCarAdsReferenceDataCommandHandler : IRequestHandler<GetCarAdsReferenceDataCommand, GetCarAdsReferenceDataResponse>
    {
        private readonly IAdStateRepository _adStateRepository;
        private readonly ICarBodyTypeRepository _carBodyTypeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IFuelTypeRepository _fuelTypeRepository;
        private readonly IGearboxTypeRepository _gearboxTypeRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IExteriorTypeRepository _exteriorTypeRepository;
        private readonly IInsidesTypesRepository _insidesTypesRepository;
        private readonly ISafetyTypeRepository _safetyTypeRepository;

        public GetCarAdsReferenceDataCommandHandler
            (
                IAdStateRepository adStateRepository,
                ICarBodyTypeRepository carBodyTypeRepository,
                IColorRepository colorRepository,
                IFuelTypeRepository fuelTypeRepository,
                IGearboxTypeRepository gearboxTypeRepository,
                IProvinceRepository provinceRepository,
                IExteriorTypeRepository exteriorTypeRepository,
                IInsidesTypesRepository insidesTypesRepository,
                ISafetyTypeRepository safetyTypeRepository
            )
        {
            _adStateRepository = adStateRepository;
            _carBodyTypeRepository = carBodyTypeRepository;
            _colorRepository = colorRepository;
            _fuelTypeRepository = fuelTypeRepository;
            _gearboxTypeRepository = gearboxTypeRepository;
            _provinceRepository = provinceRepository;
            _exteriorTypeRepository = exteriorTypeRepository;
            _insidesTypesRepository = insidesTypesRepository;
            _safetyTypeRepository = safetyTypeRepository;
        }

        public Task<GetCarAdsReferenceDataResponse> Handle(GetCarAdsReferenceDataCommand command, CancellationToken cancellationToken)
        {
            var adStates = _adStateRepository.GetAll();
            var bodyTypes = _carBodyTypeRepository.GetAll();
            var colors = _colorRepository.GetAll();
            var fuelTypes = _fuelTypeRepository.GetAll();
            var gearBoxTypes = _gearboxTypeRepository.GetAll();
            var provinces = _provinceRepository.GetAll();
            var safetyType = _safetyTypeRepository.GetAll();
            var exteriorType = _exteriorTypeRepository.GetAll();
            var insideType = _insidesTypesRepository.GetAll();

            var response = new GetCarAdsReferenceDataResponse()
            {
                States = adStates.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                CarBodyTypes = bodyTypes.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                Colors = colors.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                FuelTypes = fuelTypes.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                GearboxTypes = gearBoxTypes.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                Provinces = provinces.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                ExteriorTypes = exteriorType.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                InsideTypes = insideType.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
                SafetyTypes = safetyType.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
            };

            return Task.FromResult(response);
        }
    }
}
