using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class CarBodyType : Enumeration
    {
        public static readonly CarBodyType Berlina = new(1, "Berlina");
        public static readonly CarBodyType Familiar = new(2, "Familiar");
        public static readonly CarBodyType Coupe = new(3, "Coupe");
        public static readonly CarBodyType Monovolumen = new(4, "Monovolumen");
        public static readonly CarBodyType Carbrio = new(5, "Carbrio");
        public static readonly CarBodyType SUV = new(6, "SUV");

        public CarBodyType(int id, string name)
         : base(id, name)
        {
        }       
    }
}