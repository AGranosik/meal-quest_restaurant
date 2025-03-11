using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects;

public sealed class Street : SimpleValueType<NotEmptyString, Street>
{
    public Street(NotEmptyString streetName) : base(streetName)
    {
    }
}