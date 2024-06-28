using Restaurant.Domain.Common.DomainImplementationTypes;
using Restaurant.Domain.Restaurants.Aggregates.Entities;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.Domain.Restaurants.Aggregates.DomainEvents
{
    public sealed record OwnerCreatedEvent(Owner owner, OwnerId id) : DomainEvent<OwnerId>(id);
}
