using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

internal class RestaurantConfiguration : IEntityTypeConfiguration<RestaurantIdMenuId>
{
    public void Configure(EntityTypeBuilder<RestaurantIdMenuId> builder)
    {
        builder.ToTable(MenuDatabaseConstants.Restaurants, MenuDatabaseConstants.Schema);

        builder.HasMany<Menu>()
            .WithOne()
            .HasForeignKey("RestaurantID")
            .HasConstraintName("FK_RestaurantId");

        builder.HasKey(r => r.Value)
            .HasName("RestaurantID");

    }
}