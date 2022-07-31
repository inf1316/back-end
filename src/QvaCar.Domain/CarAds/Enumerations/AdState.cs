using QvaCar.Seedwork.Domain;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class AdState : Enumeration
    {
        public static readonly AdState Draft = new(1, "Draft");
        public static readonly AdState Published = new(2, "Published");
        public static readonly AdState Unregistered = new(3, "Unregistered");

        public AdState(int id, string name) : base(id, name) { }
    }
}