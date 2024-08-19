using domain.Common.DomainImplementationTypes;
using MediatR;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record RestaurantCreatedEvent(Restaurant Restaurant) : DomainEvent, INotification;
}
