using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects;

public sealed class City : SimpleValueType<NotEmptyString, City>
{
    public City(string cityName) : base(cityName)
    {
    }
}