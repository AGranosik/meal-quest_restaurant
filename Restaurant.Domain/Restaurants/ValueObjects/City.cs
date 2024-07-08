using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects
{
    public class City(string cityName) : SimpleValueType<NotEmptyString, City>(cityName)
    {
    }
}
