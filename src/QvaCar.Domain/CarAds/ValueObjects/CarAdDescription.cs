using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class CarAdDescription : ValueObject
    {
        public string Value { get; } = string.Empty;

        private CarAdDescription() { }
        public CarAdDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainValidationException("Description", "Description is required.");

            if (description.Length > 500)
                throw new DomainValidationException("Description", "ContactPhoneNumber is to long.");

            Value = description;
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { Value };
        private string GetDebuggerDisplay() => Value.ToString();
    }
}