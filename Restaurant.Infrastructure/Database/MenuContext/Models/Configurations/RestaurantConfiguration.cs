using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

internal class RestaurantConfiguration : IEntityTypeConfiguration<MenuRestaurant>
{
    public void Configure(EntityTypeBuilder<MenuRestaurant> builder)
    {
        builder.ToTable(MenuDatabaseConstants.Restaurants, MenuDatabaseConstants.Schema);

        // builder.HasMany<Menu>()
        //     .WithOne()
        //     .HasForeignKey("RestaurantID")
        //     .HasConstraintName("FK_RestaurantId");

        builder.HasKey(r => r.Id)
            .HasName("RestaurantID");

        builder.Property(r => r.Id)
            .HasConversion(id => id!.Value, db => new RestaurantIdMenuId(db));
        

    }
}