using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable(MenuDatabaseConstants.Menus, MenuDatabaseConstants.Schema);

        builder.Property(m => m.Id)
            .HasConversion(id => id!.Value, db => new MenuId(db))
            .ValueGeneratedOnAdd();

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Restaurant)
            .HasConversion(r => r.Id!.Value, db => new MenuRestaurant(new RestaurantIdMenuId(db)))
            .HasColumnName("RestaurantID");
        
        // builder.Property(m => m.Restaurant)
        //     .HasConversion(r => r.Value, db => new RestaurantIdMenuId(db));
        

        builder.Property(m => m.Name)
            .HasConversion(name => name.Value.Value, db => new Name(db));

        builder.HasMany(m => m.Groups)
            .WithMany();
    }
}