using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.MenuContext.Models.Configurations
{
    internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menus", "menu");

            builder.Property(m => m.Id)
                .HasConversion(id => id!.Value, db => new MenuId(db))
                .ValueGeneratedOnAdd();

            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Restaurant)
                .WithMany()
                .HasForeignKey(r => r.Restaurant.Value)
                .HasConstraintName("RestaurantID");

            builder.Property(m => m.Name)
                .HasConversion(name => name.Value.Value, db => new Name(db));

            builder.HasMany(m => m.Groups)
                .WithMany();
        }
    }
}
