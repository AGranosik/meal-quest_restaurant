using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates.Entities
{
    public class Owner : Entity<OwnerId>
    {
        public Name Name { get; }
        public Name Surname { get; }
        public Address Address { get; }

        public static Result<Owner> Create(Name name, Name surname, Address address)
        {
            var validationResult = CreationValidation( name, surname, address);
            if (validationResult.IsFailed)
                return validationResult;

            return Result.Ok(new Owner(name, surname, address));
        }
        protected Owner() : base() { }

        protected Owner(Name name, Name surname, Address address) : base()
        {
            Name = name;
            Surname = surname;
            Address = address;
        }

        private static Result CreationValidation(Name name, Name surname, Address address)
        {
            if (name is null)
                return Result.Fail("Name cannot be null.");

            if (surname is null)
                return Result.Fail("Surname cannot be null.");

            if (address is null)
                return Result.Fail("Address cannot be null.");

            return Result.Ok();
        }
    }
}
