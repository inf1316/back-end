using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ImageType : Enumeration
    {
        public static readonly ImageType Large = new(1, "Large");
        public static readonly ImageType Small = new(2, "Small");
        public static readonly ImageType Medium = new(3, "Medium");

        public ImageType(int id, string name) : base(id, name) { }
    }

}