using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ModelVersion : ValueObject
    {
        public string Value { get; } = string.Empty;
        
        private ModelVersion() { }
        public ModelVersion(string ModelVersion)
        {
            if (string.IsNullOrWhiteSpace(ModelVersion))
                throw new DomainValidationException("ModelVersion", "Description is required.");

            if (ModelVersion.Length > 50)
                throw new DomainValidationException("ModelVersion", "ContactPhoneNumber is to long.");

            Value = ModelVersion;
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { Value };
        private string GetDebuggerDisplay() => Value.ToString();
    }
}