using domain.Common.DomainImplementationTypes;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;

namespace domainRestaurants.Aggregates.DomainEvents
{
    public sealed record OwnerCreatedEvent(Owner owner, OwnerId id) : DomainEvent<OwnerId>(id);
}
