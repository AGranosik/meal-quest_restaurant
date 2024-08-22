using domain.Restaurants.Aggregates.DomainEvents;
using infrastructure.EventStorage.DatabaseModels;
using infrastructure.EventStorage.DatabaseModels.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.EventStorage
{
    public class EventdbContext(DbContextOptions<EventdbContext> options) : DbContext(options)
    {
        public DbSet<DomainEventModel<RestaurantEvent>> RestaurantEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("events");

            modelBuilder
                .ApplyConfiguration(new RestaurantEventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
