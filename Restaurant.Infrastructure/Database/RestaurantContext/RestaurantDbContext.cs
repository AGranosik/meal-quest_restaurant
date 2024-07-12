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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<Owner> Owners { get; set; }
        //public DbSet<Address> Addresses { get; set; }
    }
}
