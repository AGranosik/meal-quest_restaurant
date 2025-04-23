using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

internal class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        const string idName = "MealID";
        builder.ToTable(MenuDatabaseConstants.Meals, MenuDatabaseConstants.Schema);
        builder.Property<int>(idName)
            .ValueGeneratedOnAdd();

        builder.HasKey(idName);

        builder.Property(m => m.Price)
            .HasConversion(p => p!.Value, db => new Price(db));

        builder.Property(m => m.Name)
            .HasConversion(name => name!.Value.Value, db => new Name(db));

        builder.HasMany<Ingredient>(MenuDatabaseConstants.Ingredients)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                MenuDatabaseConstants.MealIngredients,
                e => e.HasOne<Ingredient>().WithMany().HasForeignKey(idName),
                e => e.HasOne<Meal>().WithMany().HasForeignKey("GroupID")
            );

        builder.HasMany<Category>(MenuDatabaseConstants.Categories)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                MenuDatabaseConstants.MealCategories,
                e => e.HasOne<Category>().WithMany().HasForeignKey(idName),
                e => e.HasOne<Meal>().WithMany().HasForeignKey("CategoryID")
                );
    }
}