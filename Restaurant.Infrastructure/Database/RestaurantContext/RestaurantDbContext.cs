using domain.Restaurants.Aggregates;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.RestaurantContext
{
    public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(RestaurantDatabaseConstants.SCHEMA);

            modelBuilder
                .ApplyConfiguration(new AddressConfiguration())
                .ApplyConfiguration(new OpeningHoursConfiguration())
                .ApplyConfiguration(new OwnerConfiguration())
                .ApplyConfiguration(new RestaurantConfiguration())
                .ApplyConfiguration(new MenuConfiguration())
                .ApplyConfiguration(new WorkingDayConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
