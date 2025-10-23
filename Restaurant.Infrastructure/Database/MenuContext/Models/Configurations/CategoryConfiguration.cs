using core.SimpleTypes;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(MenuDatabaseConstants.Categories, MenuDatabaseConstants.Schema);
        builder.Property(c => c.Id)
            .HasConversion(c => c!.Value, db => new CategoryId(db))
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.Name)
            .IsUnique();
        
        builder.Property(c => c.Name)
            .HasConversion(name => name.Value, db => new NotEmptyString(db));
        
    }
}