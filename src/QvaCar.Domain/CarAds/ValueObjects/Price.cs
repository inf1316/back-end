using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Price : ValueObject
    {
        public long PriceInDollars { get; }

        private Price() { }
        protected Price(long amount)
        {
            if (amount < 100)
                throw new DomainValidationException(nameof(PriceInDollars), "Price must be greater than 100.");

            PriceInDollars = amount;
        }

        public static Price FromDollars(long amount) => new(amount);
        protected override IEnumerable<object> GetEqualityComponents() => new object[] { PriceInDollars };
        private string GetDebuggerDisplay() => $"${PriceInDollars}";
    }
}