using domain.Restaurants.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations
{
    internal class WorkingDayConfiguration : IEntityTypeConfiguration<WorkingDay>
    {
        public void Configure(EntityTypeBuilder<WorkingDay> builder)
        {
            var Id = "WorkingDayID";

            builder.ToTable("WorkingDays", "restaurant");

            builder.Property<int>(Id)
                .ValueGeneratedOnAdd();

            builder.HasKey(Id);

            builder.Property(w => w.Day)
                .IsRequired();

            builder.Property(w => w.From)
                .HasConversion(
                    from => new TimeSpan(from.Hour, from.Minute, from.Second),
                    db => new TimeOnly(db.Hours, db.Minutes)
                );

            builder.Property(w => w.To)
                .HasConversion(
                    to => new TimeSpan(to.Hour, to.Minute, to.Second),
                    db => new TimeOnly(db.Hours, db.Minutes)
                );

            builder.Property(w => w.Free)
                .IsRequired();
        }
    }
}
