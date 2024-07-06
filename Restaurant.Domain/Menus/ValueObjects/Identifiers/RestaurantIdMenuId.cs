using domain.Common.BaseTypes;

namespace domain.Menus.ValueObjects.Identifiers
{
    public class RestaurantIdMenuId(int id) : ValueObject<RestaurantIdMenuId>
    {
        public int Id { get; } = id;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            RestaurantIdMenuId other = obj as RestaurantIdMenuId;
            if (other == null) return false;
            return other.Id == Id;
        }
    }
}
