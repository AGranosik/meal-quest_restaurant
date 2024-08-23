using domain.Common.DomainImplementationTypes;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.Aggregates.DomainEvents
{
    public record MenuEvent(MenuId MenuId) : DomainEvent(MenuId?.Value)
    {
    }
}
