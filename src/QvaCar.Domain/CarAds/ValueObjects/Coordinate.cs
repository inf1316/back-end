using QvaCar.Seedwork.Domain;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace QvaCar.Domain.CarAds
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Coordinate : ValueObject
    {
        public double Latitude { get; }
        public double Longitude { get; }

        private Coordinate() { }
        protected Coordinate(double latitude, double longitude)
        {
            if (-90 > latitude || latitude > 90)
                throw new DomainValidationException(nameof(Latitude), "Latitude is invalid.");

            if (-180 > longitude || longitude > 180)
                throw new DomainValidationException(nameof(Longitude), "Longitude is invalid.");

            Latitude = latitude;
            Longitude = longitude;
        }

        public static Coordinate FromLatLon(double latitude, double longitude) => new(latitude, longitude);

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { Latitude, Longitude };
        private string GetDebuggerDisplay() => ToString();

        public override string ToString() =>
            Latitude.ToString("#0.0#######", CultureInfo.InvariantCulture) + "," +
            Longitude.ToString("#0.0#######", CultureInfo.InvariantCulture);
    }
}