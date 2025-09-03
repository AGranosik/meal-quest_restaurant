using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects;

public sealed class Description : SimpleValueType<NotEmptyString, City>
{
    public Description(NotEmptyString value) : base(value)
    {
    }
}