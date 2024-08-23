using domain.Common.DomainImplementationTypes;
using domain.Restaurants.ValueObjects.Identifiers;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public record RestaurantEvent(RestaurantId? RestaurantId) : DomainEvent(RestaurantId?.Value);
}
