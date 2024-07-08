using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects.Identifiers
{
    public class OwnerId(int id) : SimpleValueType<int, OwnerId>(id)
    {
    }
}
