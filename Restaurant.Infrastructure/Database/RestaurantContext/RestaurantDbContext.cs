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
            //move to const.
            modelBuilder.HasDefaultSchema("restuarant");

            modelBuilder
                .ApplyConfiguration(new AddressConfiguration())
                .ApplyConfiguration(new OpeningHoursConfiguration())
                .ApplyConfiguration(new OwnerConfiguration())
                .ApplyConfiguration(new RestaurantConfiguration())
                .ApplyConfiguration(new WorkingDayConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
