using domain.Common.DomainImplementationTypes;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.Aggregates.DomainEvents
{
    public sealed record MenuCreatedEvent(MenuId? MenuId) : MenuEvent(MenuId);
}
