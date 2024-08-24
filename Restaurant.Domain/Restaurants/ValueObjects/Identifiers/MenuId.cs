using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Restaurants.ValueObjects.Identifiers
{
    public class MenuId(int value) : SimpleValueType<int, MenuId>(value)
    {
    }
}
