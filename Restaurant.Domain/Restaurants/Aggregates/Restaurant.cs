using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.Domain.Restaurants.Aggregates
{
    public class Restaurant(RestaurantId id) : Aggregate<RestaurantId>(id)
    {
        // ad menu here, just id, just check if and available hours/days
    }
}
