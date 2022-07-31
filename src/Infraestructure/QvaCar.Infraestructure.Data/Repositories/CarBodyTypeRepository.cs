using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class CarBodyTypeRepository : ICarBodyTypeRepository
    {
        public IReadOnlyList<CarBodyType> GetAll() => Enumeration.GetAll<CarBodyType>().ToList();

        public CarBodyType? GetByIdOrDefault(int id) => Enumeration.GetAll<CarBodyType>().FirstOrDefault(x => x.Id == id);
    }
}
