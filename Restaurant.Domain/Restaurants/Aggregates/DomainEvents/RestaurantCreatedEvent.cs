using domain.Common.DomainImplementationTypes;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record RestaurantCreatedEvent(Restaurant Restaurant) : DomainEvent;
}
