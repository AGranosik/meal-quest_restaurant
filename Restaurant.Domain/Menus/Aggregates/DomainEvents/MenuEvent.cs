using System.Text.Json;
using domain.Common.DomainImplementationTypes;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.Aggregates.DomainEvents
{
    public abstract record MenuEvent(MenuId? MenuId) : DomainEvent(MenuId?.Value);
}
