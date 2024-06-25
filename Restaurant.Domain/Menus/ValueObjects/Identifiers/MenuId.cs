using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.Domain.Menus.ValueObjects.Identifiers
{
    public class MenuId : ValueObject<MenuId>
    {
        public RestaurantId RestaurantId { get; }
        public Name Name { get; }
        public MenuId(RestaurantId restaurantId, Name name)
        {
            RestaurantId = restaurantId ?? throw new ArgumentNullException(nameof(restaurantId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            MenuId other = obj as MenuId;
            if (other == null) return false;
            return RestaurantId == other.RestaurantId && Name == other.Name;
        }
    }
}
