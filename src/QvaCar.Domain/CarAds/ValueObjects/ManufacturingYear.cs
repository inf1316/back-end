using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ManufacturingYear : ValueObject
    {
        public int Year { get; }

        private ManufacturingYear() { }
        public ManufacturingYear(int year)
        {
            if (year < 1900)
                throw new DomainValidationException(nameof(Kilometers), "Manufacturing year must be after 1900.");

            Year = year;
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { Year };
        private string GetDebuggerDisplay() => Year.ToString();
    }
}