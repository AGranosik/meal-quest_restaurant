using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects.Identifiers
{
    public class RestaurantId(int id) : SimpleValueType<int, RestaurantId>(id)
    {
    }
}
