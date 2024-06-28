using System.Net.Http.Headers;
using FluentResults;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Restaurants.Aggregates.DomainEvents;
using Restaurant.Domain.Restaurants.ValueObjects;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.Domain.Restaurants.Aggregates.Entities
{
    public class Owner : Entity<OwnerId>
    {
        public Name Name { get; }
        public Name Surname { get; }
        public Address Address { get; }

        public static Result<Owner> Create(OwnerId id, Name name, Name surname, Address address)
        {
            var validationResult = CreationValidation(id, name, surname, address);
            if (validationResult.IsFailed)
                return validationResult;

            return Result.Ok(new Owner(id, name, surname, address));
        }

        private Owner(OwnerId id, Name name, Name surname, Address address) : base(id)
        {
            Name = name;
            Surname = surname;
            Address = address;

            _domainEvents.Add(new OwnerCreatedEvent(this, id));
        }

        private static Result CreationValidation(OwnerId id, Name name, Name surname, Address address)
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
