using domain.Restaurants.ValueObjects;

namespace infrastructure.Database.RestaurantContext.Models
{
    public class Address: domain.Restaurants.ValueObjects.Address
    {
        public Address(Street street, City city, Coordinates coordinates) : base(street, city, coordinates)
        {
            
        }
        public Address() : base()
        {
            
        }
    }
}
