using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations;

internal class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable(RestaurantDatabaseConstants.RESTAURANTS, RestaurantDatabaseConstants.SCHEMA);

        builder.Property(r => r.Id)
            .HasConversion(restaurant => restaurant!.Value, db => new RestaurantId(db))
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .HasConversion(r => r.Value.Value, db => new Name(db));

        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.Owner);

        builder.HasOne(r => r.OpeningHours);

        builder.HasMany(r => r.Menus)
            .WithOne();

        builder.HasOne<Address>(r => r.Address)
            .WithMany();
        
        builder.Property(r => r.Description)
            .HasConversion(r => r.Value.Value, db => new Description(db));
    }
}