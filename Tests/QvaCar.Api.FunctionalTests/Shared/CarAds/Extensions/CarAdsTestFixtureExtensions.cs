using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QvaCar.Api.FunctionalTests.SeedWork;
using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using QvaCar.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QvaCar.Api.FunctionalTests.Shared.CarAds.Extensions
{
    public static class CarAdsTestFixtureExtensions
    {
        public static async Task AssumeCarAdInRepository(this TestServerFixture fixture, CarAd carAd)
        {
            await fixture.ExecuteScopeAsync(async services =>
            {
                var carRepo = services.GetService<ICarAdRepository>();
                var carDbContext = services.GetService<QvaCarDbContext>();

                if (carRepo is null)
                    throw new NullReferenceException($"{nameof(ICarAdRepository)} is not registered in the IoC container.");
                if (carDbContext is null)
                    throw new NullReferenceException($"{nameof(QvaCarDbContext)} is not registered in the IoC container.");


                await carRepo.AddAsync(carAd, default);
                carDbContext.Entry(carAd).State = EntityState.Detached;
            });
        }

        public static async Task<IEnumerable<CarAd>> GetAllCarsAdsInRepository(this TestServerFixture fixture)
        {
            IEnumerable<CarAd> allCarAds = new List<CarAd>();
            await fixture.ExecuteScopeAsync(async services =>
            {
                var carDbContext = services.GetService<QvaCarDbContext>();
                if (carDbContext is null)
                    throw new NullReferenceException($"{nameof(QvaCarDbContext)} is not registered in the IoC container.");

                allCarAds = await carDbContext.Ads.AsNoTracking().ToListAsync();
            });
            return allCarAds;
        }

        public static IEnumerable<AdState> GetAllAdStateReferenceData(this TestServerFixture fixture)
        {
            var result = new List<AdState>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IAdStateRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IAdStateRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<Province> GetAllProvincesReferenceData(this TestServerFixture fixture)
        {
            var result = new List<Province>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IProvinceRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IProvinceRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<CarBodyType> GetAllCarBodyTypesReferenceData(this TestServerFixture fixture)
        {
            var result = new List<CarBodyType>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<ICarBodyTypeRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(ICarBodyTypeRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());

            });
            return result;
        }

        public static IEnumerable<Color> GetAllColorReferenceData(this TestServerFixture fixture)
        {
            var result = new List<Color>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IColorRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IColorRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<FuelType> GetFuelTypesReferenceData(this TestServerFixture fixture)
        {
            var result = new List<FuelType>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IFuelTypeRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IFuelTypeRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<GearboxType> GetGearboxTypesReferenceData(this TestServerFixture fixture)
        {
            var result = new List<GearboxType>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IGearboxTypeRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IGearboxTypeRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<ExteriorType> GetExteriorTypesReferenceData(this TestServerFixture fixture) 
        {
            var result = new List<ExteriorType>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IExteriorTypeRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IExteriorTypeRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<InsideType> GetInsideTypesReferenceData(this TestServerFixture fixture)
        {
            var result = new List<InsideType>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<IInsidesTypesRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IExteriorTypeRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }

        public static IEnumerable<SafetyType> GetSafetyTypesReferenceData(this TestServerFixture fixture)
        {
            var result = new List<SafetyType>();
            fixture.ExecuteScope(services =>
            {
                var repo = services.GetService<ISafetyTypeRepository>();
                if (repo is null)
                    throw new NullReferenceException($"{nameof(IExteriorTypeRepository)} is not registered in the IoC container.");

                result.AddRange(repo.GetAll());
            });
            return result;
        }
    }
}
