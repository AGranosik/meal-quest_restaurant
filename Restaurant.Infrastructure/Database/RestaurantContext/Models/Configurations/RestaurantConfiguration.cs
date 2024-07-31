using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations
{
    internal class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants", "restaurant");

            builder.Property(r => r.Id)
                .HasConversion(restaurant => restaurant!.Value, db => new RestaurantId(db))
                .ValueGeneratedOnAdd();

            builder.HasKey(r => r.Id);

            builder.HasOne<Owner>("Owner")
                .WithMany();

            builder.HasOne<OpeningHours>("OpeningHours");

            builder.Ignore(r => r.Menus);
        }
    }
}
