using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Strings;

namespace domain.Restaurants.ValueObjects.Identifiers
{
    public class MenuRestaurantId : ValueObject<MenuRestaurantId>
    {
        public RestaurantId RestaurantId { get; }
        public Name Name { get; }
        public MenuRestaurantId(RestaurantId restaurantId, Name name)
        {
            RestaurantId = restaurantId ?? throw new ArgumentNullException(nameof(restaurantId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            MenuRestaurantId other = obj as MenuRestaurantId;
            if (other == null) return false;
            return RestaurantId == other.RestaurantId && Name == other.Name;
        }
    }
}
