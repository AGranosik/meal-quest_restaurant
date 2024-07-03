using Microsoft.EntityFrameworkCore;
using Restaurants.Infrastructure.Database.RestaurantContext.Models;

namespace Restaurants.Infrastructure.Database.RestaurantContext
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
