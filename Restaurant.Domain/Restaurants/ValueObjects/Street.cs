using Restaurant.Core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Restaurants.ValueObjects
{
    public class Street(string streetName) : ValueObject<Street>
    {
        public readonly NotEmptyString StreetName = streetName;

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Street other = obj as Street;
            if (other == null) return false;
            return StreetName == other.StreetName;
        }
    }
}
