using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class ProvinceRepository : IProvinceRepository
    {
        public IReadOnlyList<Province> GetAll() => Enumeration.GetAll<Province>().ToList();
        public Province? GetByIdOrDefault(int id) => Enumeration.GetAll<Province>().FirstOrDefault(x => x.Id == id);
    }
}