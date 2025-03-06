using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects;

public class Street(string streetName) : SimpleValueType<NotEmptyString, Street>(streetName)
{
}