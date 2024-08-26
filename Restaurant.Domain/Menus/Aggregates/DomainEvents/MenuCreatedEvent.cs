using System.Text.Json;
using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects.Identifiers;
using MediatR;

namespace domain.Menus.Aggregates.DomainEvents
{
    //validation of params should be there, refecator that when moving fro mrecord to classes
    public sealed record MenuCreatedEvent(MenuId? MenuId, Name Name, RestaurantIdMenuId Restaurant) : MenuEvent(MenuId), INotification
    {
        public override string GetAssemblyName()
            => this.GetType().AssemblyQualifiedName;

        public override string Serialize()
            => JsonSerializer.Serialize(this);
    }
}
