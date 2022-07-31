using QvaCar.Api.Features.CarAds;

namespace QvaCar.Api.FunctionalTests.Features.CarAds
{
    public class UpdateCarAdCoordinateBuilder
    {
        private double latitude;
        private double longitude;
       
        public UpdateCarAdCoordinateBuilder WithLatitude(double value)
        {
            this.latitude = value;
            return this;
        }
        
        public UpdateCarAdCoordinateBuilder WithLongitude(double value)
        {
            this.longitude = value;
            return this;
        }
        
      
        public UpdateCarAdCoordinate Build()
        {
            return new UpdateCarAdCoordinate()
            {
                Latitude = latitude,
                Longitude = longitude,
            };
        }
    }
}
