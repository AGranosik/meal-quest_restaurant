using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Restaurants.ValueObjects;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.Domain.Restaurants.Entities
{
    public class Owner(OwnerId id, Name name, Name surname, Address address) : Entity<OwnerId>(id)
    {
        public Name Name { get; } = name ?? throw new ArgumentNullException(nameof(Name));
        public Name Surname { get; } = surname ?? throw new ArgumentNullException(nameof(Surname));
        public Address Address { get; } = address ?? throw new ArgumentNullException(nameof(Address));
    }
}
