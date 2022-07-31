using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class InsideType : Enumeration
    {
        public static readonly InsideType RadioDisplay = new(1, "Radio con Pantalla");
        public static readonly InsideType SteeringWheelCommands = new(2, "Comandos al volante");
        public static readonly InsideType UpholsteredLeather = new(3, "Tapizado de Cuero");
        public static readonly InsideType UpholsteredFabric = new(4, "Tapizado de Tela");
        public static readonly InsideType ExternalViewMirrors = new(5, "Espejos retrovisores externos eléctricos");
        public static readonly InsideType RaiseGlass = new(6, "Levanta vidrios delanteros/traseros eléctricos");
        public static readonly InsideType AirConditioning = new(7, "Aire acondicionado");
        public static readonly InsideType DoorLock = new(8, "Bloqueo de puertas eléctricos");
        public static readonly InsideType ThreeRow = new(9, "3 HILERAS");
        public static readonly InsideType FoursRow = new(10, "4 HILERAS");

        public InsideType(int id, string name)
       : base(id, name) { }
    }
}
