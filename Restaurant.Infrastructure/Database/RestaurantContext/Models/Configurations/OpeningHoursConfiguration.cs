using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations
{
    internal class OpeningHoursConfiguration : IEntityTypeConfiguration<OpeningHours>
    {
        public void Configure(EntityTypeBuilder<OpeningHours> builder)
        {
            var idName = "OpeningHoursID";
            builder.ToTable("OpeningHours", "restaurant");
            builder.Property<int>(idName)
                .ValueGeneratedOnAdd();

            builder.HasKey(idName);

            builder.Property(oh => oh.From)
                .HasConversion(
                to => new TimeSpan(to.Hour, to.Minute, to.Second),
                db => new TimeOnly(db.Hours, db.Minutes)
                );

            builder.Property(oh => oh.To)
                .HasConversion(
                    to => new TimeSpan(to.Hour, to.Minute, to.Second),
                    db => new TimeOnly(db.Hours, db.Minutes)
                );
        }
    }
}
