using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects.Identifiers;

public class RestaurantId : SimpleValueType<int, RestaurantId>
{
    public RestaurantId(int value) : base(value)
    {
    }
}