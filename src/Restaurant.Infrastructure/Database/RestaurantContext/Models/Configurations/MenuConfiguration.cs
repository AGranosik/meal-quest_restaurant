using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable(RestaurantDatabaseConstants.MENUS, RestaurantDatabaseConstants.SCHEMA);

        builder.Property(m => m.Id)
            .HasConversion(id => id!.Value, db => new MenuId(db));

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasConversion(name => name.Value.Value, db => new Name(db));
    }
}