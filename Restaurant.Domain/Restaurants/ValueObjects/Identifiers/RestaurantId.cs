using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects.Identifiers;

public class RestaurantId(int value) : SimpleValueType<int, RestaurantId>(value)
{
}