using domain.Common.DomainImplementationTypes;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public record RestaurantEvent(int RestaurantId) : DomainEvent(RestaurantId);
}
