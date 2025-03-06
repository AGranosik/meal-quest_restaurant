using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers;

public class RestaurantIdMenuId : SimpleValueType<int, RestaurantIdMenuId>
{
    public RestaurantIdMenuId(int value) : base(value)
    {
            
    }

    private RestaurantIdMenuId() : base(0) { }
}