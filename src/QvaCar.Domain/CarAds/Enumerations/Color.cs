using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Color : Enumeration
    {
        public static readonly Color Blanco = new(1, "Blanco");
        public static readonly Color Negro = new(2, "Negro");
        public static readonly Color Azul = new(3, "Azul");
        public static readonly Color Rojo = new(4, "Rojo");
        public static readonly Color Verde = new(5, "Verde");
        public static readonly Color Amarillo = new(6, "Amarillo");
        public static readonly Color Carmelita = new(7, "Carmelita");

        public Color(int id, string name) : base(id, name) { }
    }
}