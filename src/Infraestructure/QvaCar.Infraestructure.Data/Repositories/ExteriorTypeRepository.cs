using QvaCar.Domain.CarAds;
using QvaCar.Domain.CarAds.Repositories;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class ExteriorTypeRepository : IExteriorTypeRepository
    {
        public IReadOnlyList<ExteriorType> GetAll() => Enumeration.GetAll<ExteriorType>().ToList();

        public ExteriorType? GetByIdOrDefault(int id) => Enumeration.GetAll<ExteriorType>().FirstOrDefault(x => x.Id == id);
    }
}
