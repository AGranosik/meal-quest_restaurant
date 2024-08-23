using domain.Restaurants.ValueObjects.Identifiers;
using MediatR;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record RestaurantCreatedEvent(RestaurantId? RestaurantId) : RestaurantEvent(RestaurantId), INotification;
}
