using core.SimpleTypes;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations
{
    internal class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            var idName = "IngredientID";
            builder.ToTable(MenuDatabaseConstants.INGREDIENTS, MenuDatabaseConstants.SCHEMA);
            builder.Property<int>(idName)
                .ValueGeneratedOnAdd();

            builder.HasKey(idName);

            builder.Property(i => i.Name)
                .HasConversion(name => name.Value, db => new NotEmptyString(db));
        }
    }
}
