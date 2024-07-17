using domain.Restaurants.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses", "restaurant");
            builder.Property<int>("AddressID")
                .ValueGeneratedOnAdd();

            builder.HasKey("AddressID");

            builder.Property(x => x.Street)
                .HasConversion(street => street.Value.Value, db => new Street(db));

            builder.Property(x => x.City)
                .HasConversion(city => city.Value.Value, db => new City(db));

            builder.OwnsOne(a => a.Coordinates, coordinates =>
            {
                coordinates.Property(c => c.X);
                coordinates.Property(c => c.Y);
            });
        }
    }
}
