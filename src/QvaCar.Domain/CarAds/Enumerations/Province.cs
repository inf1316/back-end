using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Province : Enumeration
    {
        public static readonly Province PinarDelRio = new(1, "Pinar del Río");
        public static readonly Province Artemisa = new(2, "Artemisa");
        public static readonly Province LaHabana = new(3, "La Habana");
        public static readonly Province Mayabeque = new(4, "Mayabeque");
        public static readonly Province Matanzas = new(5, "Matanzas");
        public static readonly Province Cienfuegos = new(6, "Cienfuegos");
        public static readonly Province VillaClara = new(7, "Villa Clara");
        public static readonly Province SanctiSpíritus = new(8, "Sancti Spíritus");
        public static readonly Province CiegoDeÁvila = new(9, "Ciego de Ávila");
        public static readonly Province Camaguey = new(10, "Camagüey");
        public static readonly Province LasTunas = new(11, "Las Tunas");
        public static readonly Province Granma = new(12, "Granma");
        public static readonly Province Holguin = new(13, "Holguín");
        public static readonly Province SantiagoDeCuba = new(14, "Santiago de Cuba");
        public static readonly Province Guantanamo = new(15, "Guantánamo");
        public static readonly Province IslaDeLaJuventud = new(16, "Municipio Especial Isla de la Juventud");
        public Province(int id, string name) : base(id, name) { }
    }
}