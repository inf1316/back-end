using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class SafetyType : Enumeration
    {
        public static readonly SafetyType Alarma = new(1, "Alarma");
        public static readonly SafetyType Airbags = new(2, "Airbags");
        public static readonly SafetyType FrenosAbs = new(3, "Frenos ABS");
        public static readonly SafetyType SensorEstacionamiento = new(4, "Sensor de Estacionamiento");
        public static readonly SafetyType CamaraRetroceso = new(5, "Camara de Retroceso");        

        public SafetyType(int id, string name)
         : base(id, name) { }
    }
}
