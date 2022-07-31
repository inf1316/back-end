using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class InsidesTypesRepository : IInsidesTypesRepository
    {
        public IReadOnlyList<InsideType> GetAll() => Enumeration.GetAll<InsideType>().ToList();

        public InsideType? GetByIdOrDefault(int id) => Enumeration.GetAll<InsideType>().FirstOrDefault(x => x.Id == id);
    }
}
