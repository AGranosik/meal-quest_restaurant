using infrastructure.Database.RestaurantContext.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace infrastructure.Database.RestaurantContext
{
    internal class RestaurantDbContext : DbContext
    {
        internal RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }

        internal DbSet<Restaurant> Restaurants { get; set; }

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
        }
    }
}
