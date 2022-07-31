using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QvaCar.Domain.CarAds;

namespace QvaCar.Infraestructure.Data
{
    internal class CarAdEntityTypeConfiguration : IEntityTypeConfiguration<CarAd>
    {
        public void Configure(EntityTypeBuilder<CarAd> builder)
        {
            builder.ToContainer(nameof(CarAd));

            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.UserId)
                .HasConversion<string>();
            builder.HasPartitionKey(m => m.UserId);

            builder
               .Property(m => m.CreatedAt);

            builder
               .Property(m => m.UpdatedAt);

            builder
                .Property(m => m.Price)
                .HasConversion(prop => prop.PriceInDollars, value => Price.FromDollars(value));

            builder
                .Property(m => m.Description)
                .HasConversion(prop => prop.Value, value => new CarAdDescription(value));

            builder
                .Property(m => m.ManufacturingYear)
                .HasConversion(prop => prop.Year, value => new ManufacturingYear(value));

            builder
                .Property(m => m.Kilometers)
                .HasConversion(prop => prop.Value, value => Kilometers.FromKilometers(value));

            builder
                .Property(m => m.ContactPhoneNumber)
                .HasConversion(prop => prop.Value, value => new CarAdContactPhoneNumber(value));

            builder
                .Property(m => m.ModelVersion)
                .HasConversion(prop => prop.Value, value => new ModelVersion(value));


            builder.OwnsOne(m => m.Province);
            builder.OwnsOne(m => m.GearboxType);
            builder.OwnsOne(m => m.BodyType);
            builder.OwnsOne(m => m.Color);
            builder.OwnsOne(m => m.FuelType);
            builder.OwnsOne(m => m.State);
            builder.OwnsOne(m => m.ContactLocation, locationBuilder =>
            {
                locationBuilder.Property(location => location.Latitude);
                locationBuilder.Property(location => location.Longitude);
            });

            builder.OwnsMany(m => m.Images);
            builder.OwnsMany(m => m.ExteriorTypes).HasKey(m => m.Id);
            builder.OwnsMany(m => m.SafetyTypes).HasKey(m => m.Id);
            builder.OwnsMany(m => m.InsideTypes).HasKey(m => m.Id);
        }
    }
}
