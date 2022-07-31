using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ExteriorType : Enumeration
    {
        public static readonly ExteriorType SportTires = new(1, "Llantas Deportivas");
        public static readonly ExteriorType Polarized = new(2, "Polarizado");
        public static readonly ExteriorType LookFootPrint = new(3, "Busca Huellas");
        public static readonly ExteriorType SolarRoof = new(4, "Techo Solar");        

        public ExteriorType(int id, string name)
        : base(id, name) { }
    }
}
