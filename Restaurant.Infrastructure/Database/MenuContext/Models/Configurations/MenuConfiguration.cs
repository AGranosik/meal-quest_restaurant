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
        builder.ToTable(MenuDatabaseConstants.MENUS, MenuDatabaseConstants.SCHEMA);

        builder.Property(m => m.Id)
            .HasConversion(id => id!.Value, db => new MenuId(db))
            .ValueGeneratedOnAdd();

        builder.HasKey(m => m.Id);

        builder.Ignore(m => m.Restaurant);

        builder.Property(m => m.Name)
            .HasConversion(name => name.Value.Value, db => new Name(db));

        builder.HasMany(m => m.Groups)
            .WithMany();
    }
}