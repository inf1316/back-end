using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class CarAdContactPhoneNumber : ValueObject
    {
        public string Value { get; } = string.Empty;

        private CarAdContactPhoneNumber() { }
        public CarAdContactPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new DomainValidationException("PhoneNumber", "ContactPhoneNumber is required.");

            if (phoneNumber.Length > 20)
                throw new DomainValidationException("PhoneNumber", "ContactPhoneNumber is to long.");

            Value = phoneNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { Value };
        private string GetDebuggerDisplay() => Value.ToString();
    }
}