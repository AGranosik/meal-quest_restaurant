using domain.Common.DomainImplementationTypes;
using infrastructure.EventStorage.DatabaseModels;
using infrastructure.EventStorage.DatabaseModels.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.EventStorage
{
    public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
    {
        public DbSet<DomainEventModel<TDomainEvent>> GetDbSet<TDomainEvent>()
            where TDomainEvent : DomainEvent
                => Set<DomainEventModel<TDomainEvent>>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("events");

            modelBuilder
                .ApplyConfiguration(new RestaurantEventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
