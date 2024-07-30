using infrastructure.Database.RestaurantContext.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace infrastructure.Database.RestaurantContext
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IgnoreDomainModels(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        private static void IgnoreDomainModels(ModelBuilder modelBuilder)
        {
            // if there will occur more cases use reflection
            modelBuilder.Ignore<domain.Restaurants.ValueObjects.WorkingDay>();
            modelBuilder.Ignore<domain.Restaurants.ValueObjects.OpeningHours>();
            modelBuilder.Ignore<domain.Restaurants.Aggregates.Entities.Owner>();
            modelBuilder.Ignore<domain.Restaurants.Aggregates.Restaurant>();
        }
    }
}
