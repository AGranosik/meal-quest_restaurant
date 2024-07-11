using domain.Common.BaseTypes;

namespace domain.Restaurants.ValueObjects
{
    public class Address : ValueObject<Address>
    {
        public Address(Street street, City city, Coordinates coordinates)
        {
            Street = street ?? throw new ArgumentNullException();
            City = city ?? throw new ArgumentNullException();
            Coordinates = coordinates ?? throw new ArgumentNullException();
        }

        protected Address() { }

        public Street Street { get; }
        public City City { get; }
        public Coordinates Coordinates { get; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Address? other = obj as Address;
            if (other == null) return false;
            return Street == other.Street && City == other.City && Coordinates == other.Coordinates;
        }
    }
}
