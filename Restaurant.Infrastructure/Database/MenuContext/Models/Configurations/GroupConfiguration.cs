using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations;

internal class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        var idName = "GroupID";
        builder.ToTable(MenuDatabaseConstants.Groups, MenuDatabaseConstants.Schema);
        builder.Property<int>(idName)
            .ValueGeneratedOnAdd();

        builder.Property(g => g.GroupName)
            .HasConversion(name => name.Value.Value, db => new Name(db));

        builder.HasMany(g => g.Meals)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                MenuDatabaseConstants.GroupMeals,
                e => e.HasOne<Meal>().WithMany().HasForeignKey("MealID"),
                e => e.HasOne<Group>().WithMany().HasForeignKey("GroupID")
            );
    }
}