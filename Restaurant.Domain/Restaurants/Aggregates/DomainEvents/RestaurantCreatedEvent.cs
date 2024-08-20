using domain.Common.DomainImplementationTypes;
using domain.Restaurants.ValueObjects.Identifiers;
using MediatR;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record RestaurantCreatedEvent(RestaurantId Id) : DomainEvent, INotification;
}
