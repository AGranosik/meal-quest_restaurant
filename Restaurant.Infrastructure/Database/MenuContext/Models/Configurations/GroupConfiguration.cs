using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            var idName = "GroupID";
            builder.ToTable("Groups", "menu");
            builder.Property<int>(idName)
                .ValueGeneratedOnAdd();

            builder.Property(g => g.GroupName)
                .HasConversion(name => name.Value.Value, db => new Name(db));

            builder.HasMany<Meal>("Meals")
                .WithMany();
        }
    }
}
