using System.Text.Json;
using System.Text.Json.Serialization;
using domain.Restaurants.ValueObjects.Identifiers;
using MediatR;

namespace domain.Restaurants.Aggregates.DomainEvents
{
    public sealed class RestaurantCreatedEvent : RestaurantEvent, INotification
    {
        public RestaurantCreatedEvent(RestaurantId? restaurantId) : base(restaurantId?.Value) { }

        [JsonConstructor]
        private RestaurantCreatedEvent(int? streamId) : base(streamId) { }

        public override string GetAssemblyName()
            => GetType().AssemblyQualifiedName;
        public override string Serialize()
            => JsonSerializer.Serialize(this);
    }
}
