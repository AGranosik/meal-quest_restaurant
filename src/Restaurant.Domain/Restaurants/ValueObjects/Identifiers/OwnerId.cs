using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects.Identifiers;

public class OwnerId : SimpleValueType<int, OwnerId>
{
    public OwnerId(int id) : base(id)
    {
    }
}