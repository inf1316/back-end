using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Kilometers : ValueObject
    {
        public int Value { get; }

        private Kilometers() { }
        protected Kilometers(int kilometers)
        {
            if (kilometers < 0)
                throw new DomainValidationException(nameof(Kilometers), "Kilometers must be at least 0.");

            Value = kilometers;
        }

        public static Kilometers FromKilometers(int kilometers) => new(kilometers);
        protected override IEnumerable<object> GetEqualityComponents() => new object[] { Value };
        private string GetDebuggerDisplay() => Value.ToString();
    }
}