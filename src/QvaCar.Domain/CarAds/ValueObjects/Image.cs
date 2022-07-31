using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Image : ValueObject
    {
        public string FileName { get; set; }

#nullable disable
        private Image() { }
#nullable enable

        public Image(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new DomainValidationException(nameof(FileName), "FileName is required.");

            FileName = fileName;
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { FileName };

        private string GetDebuggerDisplay() => FileName.ToString();
    }
}
