using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class FuelTypeRepository : IFuelTypeRepository
    {
        public IReadOnlyList<FuelType> GetAll() => Enumeration.GetAll<FuelType>().ToList();

        public FuelType? GetByIdOrDefault(int id) => Enumeration.GetAll<FuelType>().FirstOrDefault(x => x.Id == id);
    }
}
