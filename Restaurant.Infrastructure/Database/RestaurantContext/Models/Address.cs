using domain.Restaurants.ValueObjects;

namespace infrastructure.Database.RestaurantContext.Models
{
    public class Address: domain.Restaurants.ValueObjects.Address
    {
        private Address(Street street, City city, Coordinates coordinates) : base(null, null, null)
        {
            
        }
        public Address() : base()
        {
            
        }
    }
}
