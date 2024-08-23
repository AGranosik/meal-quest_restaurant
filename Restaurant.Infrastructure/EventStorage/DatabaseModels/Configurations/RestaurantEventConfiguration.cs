using domain.Restaurants.Aggregates.DomainEvents;

namespace infrastructure.EventStorage.DatabaseModels.Configurations
{
    internal class RestaurantEventConfiguration : DomainEventModelConfiguration<RestaurantEvent>
    {
    }
}
