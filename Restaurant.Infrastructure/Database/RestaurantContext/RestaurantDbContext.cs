using domain.Restaurants.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace infrastructure.Database.RestaurantContext
{
    public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //move to const.
            modelBuilder.HasDefaultSchema("restuarant");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
