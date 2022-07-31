using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Infraestructure.Data.Repositories
{
    public class AdStateRepository : IAdStateRepository
    {
        public IReadOnlyList<AdState> GetAll() => Enumeration.GetAll<AdState>().ToList();

        public AdState? GetByIdOrDefault(int id) => Enumeration.GetAll<AdState>().FirstOrDefault(x => x.Id == id);
    }
}
