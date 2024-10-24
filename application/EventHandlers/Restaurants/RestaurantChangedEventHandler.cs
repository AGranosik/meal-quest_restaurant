using application.EventHandlers.Interfaces;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;

namespace application.EventHandlers.Restaurants
{
    public class RestaurantChangedEventHandler(IEventInfoStorage<Restaurant, RestaurantId> eventInfoStorage) : AggregateChangedEventHandler<Restaurant, RestaurantId>(eventInfoStorage)
    {
        protected override Task ProcessingEventAsync(AggregateChangedEvent<Restaurant, RestaurantId> notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
