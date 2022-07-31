using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    internal class ColorRepository : IColorRepository
    {
        public IReadOnlyList<Color> GetAll() => Enumeration.GetAll<Color>().ToList();

        public Color? GetByIdOrDefault(int id) => Enumeration.GetAll<Color>().FirstOrDefault(x => x.Id == id);
    }
}
