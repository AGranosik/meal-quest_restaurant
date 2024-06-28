using Restaurant.Domain.Common.DomainImplementationTypes;
using Restaurant.Domain.Menus.Aggregates.Entities;
using Restaurant.Domain.Menus.ValueObjects.Identifiers;

namespace Restaurant.Domain.Menus.Aggregates.DomainEvents
{
    public sealed record MenuCreatedEvent(Menu Menu, MenuId Id) : DomainEvent<MenuId>(Id);
}
