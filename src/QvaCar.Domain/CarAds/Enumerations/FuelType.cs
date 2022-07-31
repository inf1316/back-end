using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class FuelType : Enumeration
    {
        public static readonly FuelType Gasolina = new(1, "Gasolina");
        public static readonly FuelType Diesel = new(2, "Diesel");
        public static readonly FuelType Hibrido = new(3, "Hibrido");
        public FuelType(int id, string name) : base(id, name) { }
    }
}