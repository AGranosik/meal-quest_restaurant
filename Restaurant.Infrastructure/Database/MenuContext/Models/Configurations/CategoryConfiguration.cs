using core.SimpleTypes;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{


    public void Configure(EntityTypeBuilder<Category> builder)
    {
        var idName = "CategoryID";
        builder.ToTable(MenuDatabaseConstants.Categories, MenuDatabaseConstants.Schema);
        builder.Property<int>(idName)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Value)
            .HasConversion(name => name.Value, db => new NotEmptyString(db));
    }
}