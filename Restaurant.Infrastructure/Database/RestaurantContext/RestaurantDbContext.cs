using Microsoft.EntityFrameworkCore;
using infrastructure.Database.RestaurantContext.Models;

namespace infrastructure.Database.RestaurantContext
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
