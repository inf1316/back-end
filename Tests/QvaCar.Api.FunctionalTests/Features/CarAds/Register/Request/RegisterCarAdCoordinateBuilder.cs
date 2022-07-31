using QvaCar.Api.Features.CarAds;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    public class RegisterCarAdCoordinateBuilder
    {
        private double latitude;
        private double longitude;
       
        public RegisterCarAdCoordinateBuilder WithLatitude(double value)
        {
            this.latitude = value;
            return this;
        }
        
        public RegisterCarAdCoordinateBuilder WithLongitude(double value)
        {
            this.longitude = value;
            return this;
        }
        
      
        public RegisterCarAdCoordinate Build()
        {
            return new RegisterCarAdCoordinate()
            {
                Latitude = latitude,
                Longitude = longitude,
            };
        }
    }
}
