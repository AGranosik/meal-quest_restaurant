using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations
{
    internal class MealConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            var idName = "MealID";
            builder.ToTable("Meals", "menu");
            builder.Property<int>(idName)
                .ValueGeneratedOnAdd();

            builder.HasKey(idName);

            builder.Property(m => m.Price)
                .HasConversion(p => p.Value, db => new Price(db));

            builder.Property(m => m.Name)
                .HasConversion(name => name.Value.Value, db => new Name(db));

            builder.HasMany<Ingredient>("Ingredients")
                .WithMany();
        }
    }
}
