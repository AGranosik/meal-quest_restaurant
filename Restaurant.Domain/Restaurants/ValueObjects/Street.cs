using Restaurant.Core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Restaurants.ValueObjects
{
    public class Street(string streetName) : ValueObject<Street>
    {
        private readonly NotEmptyString _streetName = streetName;

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
