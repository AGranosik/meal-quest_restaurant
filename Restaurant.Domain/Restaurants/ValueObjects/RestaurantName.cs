using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects
{
    public class RestaurantName : SimpleValueType<NotEmptyString, RestaurantName>
    {
        public RestaurantName(string restaurantName) : base(restaurantName)
        {
        }
    }
}
