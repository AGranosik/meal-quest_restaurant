using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations
{
    internal class RestaurantConfiguration : IEntityTypeConfiguration<RestaurantIdMenuId>
    {
        public void Configure(EntityTypeBuilder<RestaurantIdMenuId> builder)
        {
            builder.ToTable("Restaurants", "menu");

            builder.HasMany<Menu>()
                .WithOne();

            builder.HasKey(r => r.Value)
                .HasName("RestaurantID");
        }
    }
}
