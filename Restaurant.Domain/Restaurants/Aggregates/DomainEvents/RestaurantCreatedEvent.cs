using domain.Restaurants.ValueObjects.Identifiers;
using MediatR;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    //find a way no nto store uneccessary data with events... // base on class no records
    public sealed record RestaurantCreatedEvent(RestaurantId? RestaurantId) : RestaurantEvent(RestaurantId), INotification;
}
