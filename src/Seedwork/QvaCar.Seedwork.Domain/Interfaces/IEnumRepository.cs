using System.Collections.Generic;

namespace QvaCar.Seedwork.Domain
{
    public interface IEnumRepository<T> where T : Enumeration
    {
        IReadOnlyList<T> GetAll();
        T? GetByIdOrDefault(int id);
    }
}