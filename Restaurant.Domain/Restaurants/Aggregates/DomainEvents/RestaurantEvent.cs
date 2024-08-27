using domain.Common.DomainImplementationTypes;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public abstract class RestaurantEvent(int? streamId) : DomainEvent(streamId)
    {
    }
}
