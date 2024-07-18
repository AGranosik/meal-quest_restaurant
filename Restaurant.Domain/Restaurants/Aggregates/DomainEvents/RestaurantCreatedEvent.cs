using domain.Common.DomainImplementationTypes;
using domain.Restaurants.ValueObjects.Identifiers;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record RestaurantCreatedEvent(Restaurant Restaurant) : DomainEvent<RestaurantId>(Restaurant.Id)
}
