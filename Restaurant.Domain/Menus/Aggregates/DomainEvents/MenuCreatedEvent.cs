using System.Text.Json;
using System.Text.Json.Serialization;
using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects.Identifiers;
using MediatR;

namespace domain.Menus.Aggregates.DomainEvents
{
    public sealed class MenuCreatedEvent : MenuEvent, INotification
    {
        public MenuCreatedEvent(MenuId? menuId, Name name, RestaurantIdMenuId restaurant) : base(menuId?.Value)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(restaurant);

            Name = name.Value.Value;
            RestaurantId = restaurant.Value;
        }

        [JsonConstructor]
        private MenuCreatedEvent(int? streamId, string name, int restaurantId) : base(streamId)
        {
            Name = name;
            RestaurantId = restaurantId;
        }
        public string Name { get; }
        public int RestaurantId { get; }

        public override string GetAssemblyName()
            => GetType().AssemblyQualifiedName!;

        public override string Serialize()
            => JsonSerializer.Serialize(this);
    }
}
