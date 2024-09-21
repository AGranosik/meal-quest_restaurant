using domain.Restaurants.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations
{
    internal class OpeningHoursConfiguration : IEntityTypeConfiguration<OpeningHours>
    {
        public void Configure(EntityTypeBuilder<OpeningHours> builder)
        {
            var idName = "OpeningHoursID";
            builder.ToTable(RestaurantDatabaseConstants.OPENINGHOURS, RestaurantDatabaseConstants.SCHEMA);
            builder.Property<int>(idName)
                .ValueGeneratedOnAdd();

            builder.HasKey(idName);

            builder.HasMany<WorkingDay>("WorkingDays")
                .WithOne();

        }
    }
}
