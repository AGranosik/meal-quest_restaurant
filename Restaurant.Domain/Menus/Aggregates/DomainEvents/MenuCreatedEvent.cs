using domain.Common.DomainImplementationTypes;
using domain.Menus.Aggregates.Entities;

namespace domain.Menus.Aggregates.DomainEvents
{
    public sealed record MenuCreatedEvent(Menu Menu) : DomainEvent;
}
