using domain.Common.DomainImplementationTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using infrastructure.EventStorage.DatabaseModels;
using infrastructure.EventStorage.DatabaseModels.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.EventStorage;

internal class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
    {
    }

    public DbSet<DomainEventModel<TAggregate, TKey>> GetDbSet<TAggregate, TKey>()
        where TAggregate : Aggregate<TKey>
        where TKey : SimpleValueType<int, TKey>
        => Set<DomainEventModel<TAggregate, TKey>>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.SCHEMA);

        modelBuilder
            .ApplyConfiguration(new RestaurantEventConfiguration())
            .ApplyConfiguration(new MenuEventConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}