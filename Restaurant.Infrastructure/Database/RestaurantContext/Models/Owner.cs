using domain.Common.ValueTypes.Strings;
using domain.Restaurants.ValueObjects.Identifiers;

namespace infrastructure.Database.RestaurantContext.Models
{
    public class Owner : domain.Restaurants.Aggregates.Entities.Owner
    {
        private Owner(OwnerId id, Name name, Name surname, Address address) : base(null, null, null, null)
        {

        }

        public Owner() : base()
        {
            
        }
    }
}
