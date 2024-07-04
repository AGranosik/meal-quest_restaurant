using domain.Common.BaseTypes;

namespace domain.Restaurants.ValueObjects.Identifiers
{
    public class OwnerId(int id) : ValueObject<OwnerId>
    {
        public int Id { get; } = id;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            OwnerId other = obj as OwnerId;
            if (other == null) return false;
            return other.Id == Id;
        }
    }
}
