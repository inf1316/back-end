using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class GearboxType : Enumeration
    {
        public static readonly GearboxType Manual = new(1, "Manual");
        public static readonly GearboxType Automatico = new(2, "Automático");
        public static readonly GearboxType Secuencial = new(3, "Secuencial");
        public GearboxType(int id, string name) : base(id, name) { }
    }
}