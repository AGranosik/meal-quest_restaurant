using domain.Common.BaseTypes;

namespace unitTests.Domain.Menus.ValueObejcts.Identifiers
{
    public class RestaurantMenuId(int id) : ValueObject<RestaurantMenuId>
    {
        public int Id { get; } = id;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            RestaurantMenuId other = obj as RestaurantMenuId;
            if (other == null) return false;
            return other.Id == Id;
        }
    }
}
