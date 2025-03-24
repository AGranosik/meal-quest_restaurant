using domain.Common.DomainImplementationTypes;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.ValueObjects;

public sealed class MenuRestaurant : Entity<RestaurantIdMenuId>
{
    public MenuRestaurant(RestaurantIdMenuId id) :  base(id)
    {
        
    }
}