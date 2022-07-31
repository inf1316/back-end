using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class GearboxTypeRepository : IGearboxTypeRepository
    {
        public IReadOnlyList<GearboxType> GetAll() => Enumeration.GetAll<GearboxType>().ToList();

        public GearboxType? GetByIdOrDefault(int id) => Enumeration.GetAll<GearboxType>().FirstOrDefault(x => x.Id == id);
    }
}
