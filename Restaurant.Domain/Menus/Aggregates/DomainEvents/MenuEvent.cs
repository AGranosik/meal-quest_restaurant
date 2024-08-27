using System.Text.Json;
using domain.Common.DomainImplementationTypes;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.Aggregates.DomainEvents
{
    public abstract class MenuEvent(int? StreamId) : DomainEvent(StreamId)
    {
    }
}
