using domain.Restaurants.Aggregates.DomainEvents;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.EventStorage.DatabaseModels.Configurations
{
    internal class RestaurantEventConfiguration : DomainEventModelConfiguration<RestaurantEvent>
    {
        public override void Configure(EntityTypeBuilder<DomainEventModel<RestaurantEvent>> builder)
        {
            base.Configure(builder);
        }
    }
}
