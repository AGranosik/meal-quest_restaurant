using domain.Restaurants.ValueObjects;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Restaurants.Aggregates.Entities;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.Domain.Restaurants.Aggregates
{
    public class Restaurant: Aggregate<RestaurantId>
    {
        private Restaurant(RestaurantId id, Owner owner, OpeningHours openingHours) : base(id)
        {
            
        }
    }
}
