using Microsoft.EntityFrameworkCore;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data.Queries
{
    internal class CarAdsQueries : ICarAdsQueries
    {
        private readonly QvaCarDbContext _dbContext;

        public CarAdsQueries(QvaCarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<GetCarAdListItemQueryResponse>> GetCarAdsByUserAsync(Guid userId, CancellationToken cancellationToken)
        {            
            var userAds = await _dbContext.Ads
                .AsNoTracking()
                .WithPartitionKey(userId.ToString())
                .Where(x => x.UserId == userId && x.State.Id != AdState.Unregistered.Id)
                .OrderBy(x=> x.CreatedAt)
                .ToListAsync(cancellationToken);

            List<GetCarAdListItemQueryResponse> response = new();

            foreach (var rawAd in userAds)
            {
                response.Add(new GetCarAdListItemQueryResponse()
                {
                    Id = rawAd.Id,
                    UserId = rawAd.UserId,
                    CreatedAt = rawAd.CreatedAt,
                    UpdatedAt = rawAd.UpdatedAt,
                    StateId = rawAd.State.Id,
                    StateName = rawAd.State.Name,

                    Price = rawAd.Price.PriceInDollars,
                    ManufacturingYear = rawAd.ManufacturingYear.Year,
                    Kilometers = rawAd.Kilometers.Value,
                    Description = rawAd.Description.Value,
                    ContactPhoneNumber = rawAd.ContactPhoneNumber.Value,
                    ModelVersion = rawAd.ModelVersion.Value,

                    ProvinceId = rawAd.Province.Id,
                    ProvinceName = rawAd.Province.Name,

                    BodyTypeId = rawAd.BodyType.Id,
                    BodyTypeName = rawAd.BodyType.Name,

                    ColorId = rawAd.Color.Id,
                    ColorName = rawAd.Color.Name,

                    FuelTypeId = rawAd.FuelType.Id,
                    FuelTypeName = rawAd.FuelType.Name,

                    GearboxTypeId = rawAd.GearboxType.Id,
                    GearboxTypeName = rawAd.GearboxType.Name,

                    ContactLocation = GetListResponseLocationFromDomain(rawAd.ContactLocation),

                    ExteriorTypes = rawAd.ExteriorTypes.Select(r => new BaseItemListQueryResponse() { Id = r.Id, Name = r.Name}).ToArray(),
                    SafetyTypes = rawAd.SafetyTypes.Select(r => new BaseItemListQueryResponse() { Id = r.Id, Name = r.Name }).ToArray(),
                    InsideTypes = rawAd.InsideTypes.Select(r => new BaseItemListQueryResponse() { Id = r.Id, Name = r.Name }).ToArray(),

                    Images = rawAd.Images.Select(r => r.FileName).ToArray()
                }); 
            }
            return response;
        }

        public async Task<GetCarAdByIdQueryResponse?> GetCarAdByUserAndAdIdAsync(Guid userId, Guid adId, CancellationToken cancellationToken)
        {
            var rawAd = await _dbContext.Ads
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Id == adId && x.State.Id != AdState.Unregistered.Id)
                .WithPartitionKey(userId.ToString())
                .FirstOrDefaultAsync(cancellationToken);

            if (rawAd.IsNull()) return null;

            return new GetCarAdByIdQueryResponse()
            {
                Id = rawAd.Id,
                UserId = rawAd.UserId,
                CreatedAt = rawAd.CreatedAt,
                UpdatedAt = rawAd.UpdatedAt,
                StateId = rawAd.State.Id,
                StateName = rawAd.State.Name,

                Price = rawAd.Price.PriceInDollars,
                ManufacturingYear = rawAd.ManufacturingYear.Year,
                Kilometers = rawAd.Kilometers.Value,
                Description = rawAd.Description.Value,
                ContactPhoneNumber = rawAd.ContactPhoneNumber.Value,
                ModelVersion = rawAd.ModelVersion.Value,

                ProvinceId = rawAd.Province.Id,
                ProvinceName = rawAd.Province.Name,

                BodyTypeId = rawAd.BodyType.Id,
                BodyTypeName = rawAd.BodyType.Name,


                ColorId = rawAd.Color.Id,
                ColorName = rawAd.Color.Name,

                FuelTypeId = rawAd.FuelType.Id,
                FuelTypeName = rawAd.FuelType.Name,

                GearboxTypeId = rawAd.GearboxType.Id,
                GearboxTypeName = rawAd.GearboxType.Name,
               
                ContactLocation = GetByIdResponseLocationFromDomain(rawAd.ContactLocation),
                ExteriorTypes = rawAd.ExteriorTypes.Select(r => new BaseItemByIdQueryResponse() { Id = r.Id, Name = r.Name }).ToArray(),
                SafetyTypes = rawAd.SafetyTypes.Select(r => new BaseItemByIdQueryResponse() { Id = r.Id, Name = r.Name }).ToArray(),
                InsideTypes = rawAd.InsideTypes.Select(r => new BaseItemByIdQueryResponse() { Id = r.Id, Name = r.Name }).ToArray(),

                Images = rawAd.Images.Select(r => r.FileName).ToArray()
            };
        }

        private CarAdByUserCoordinateQueryResponse? GetListResponseLocationFromDomain(Coordinate? coordinate)
        {
            if (coordinate is null)
                return null;
            return new CarAdByUserCoordinateQueryResponse() { Latitude = coordinate.Latitude, Longitude = coordinate.Longitude };
        }

        private CarAdByIdCoordinateQueryResponse? GetByIdResponseLocationFromDomain(Coordinate? coordinate)
        {
            if (coordinate is null)
                return null;
            return new CarAdByIdCoordinateQueryResponse() { Latitude = coordinate.Latitude, Longitude = coordinate.Longitude };
        }
    }
}
