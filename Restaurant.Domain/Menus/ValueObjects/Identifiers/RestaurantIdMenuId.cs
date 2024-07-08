using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers
{
    public class RestaurantIdMenuId(int id) : SimpleValueType<int, RestaurantIdMenuId>(id)
    {
    }
}
