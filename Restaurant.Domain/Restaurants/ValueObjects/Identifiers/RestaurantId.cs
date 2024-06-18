using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Restaurants.ValueObjects.Identifiers
{
    public class RestaurantId(int id) : ValueObject<RestaurantId>
    {
        public int Id { get; } = id;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            RestaurantId other = obj as RestaurantId;
            if (other == null) return false;
            return other.Id == Id;
        }
    }
}
