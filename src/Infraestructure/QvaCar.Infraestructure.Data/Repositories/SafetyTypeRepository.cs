using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class SafetyTypeRepository : ISafetyTypeRepository
    {
        public IReadOnlyList<SafetyType> GetAll() => Enumeration.GetAll<SafetyType>().ToList();

        public SafetyType? GetByIdOrDefault(int id) => Enumeration.GetAll<SafetyType>().FirstOrDefault(x => x.Id == id);
    }
}
