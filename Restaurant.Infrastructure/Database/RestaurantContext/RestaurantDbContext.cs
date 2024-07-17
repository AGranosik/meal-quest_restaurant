using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace infrastructure.Database.RestaurantContext
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IgnoreDomainModels(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        private void IgnoreDomainModels(ModelBuilder modelBuilder)
        {
            // if there will occur more cases use reflection
            modelBuilder.Ignore<domain.Restaurants.ValueObjects.WorkingDay>();
        }
    }
}
