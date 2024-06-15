using Restaurant.Core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Restaurants.ValueObjects
{
    public class Address(Street street, City city) : ValueObject<Address>
    {
        public Street Street { get; } = street ?? throw new ArgumentNullException();
        public City City { get; } = city ?? throw new ArgumentNullException();

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Address? other = obj as Address;
            if (other == null) return false;
            return Street == other.Street && City == other.City;
        }
    }
}
