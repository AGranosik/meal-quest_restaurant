using domain.Restaurants.ValueObjects.Identifiers;
using MediatR;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record RestaurantCreatedEvent(RestaurantId Id) : RestaurantEvent(Id.Value), INotification;
}
