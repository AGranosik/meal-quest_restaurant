using domain.Restaurants.ValueObjects.Identifiers;

namespace infrastructure.Database.RestaurantContext.Models
{
    internal class Restaurant : domain.Restaurants.Aggregates.Restaurant
    {
        // name
        // multiple opening hours
        private Restaurant(RestaurantId id, domain.Restaurants.Aggregates.Entities.Owner owner, domain.Restaurants.ValueObjects.OpeningHours openingHours) : base(null, null, null) { }

        public Restaurant() : base(){ }
    }
}
