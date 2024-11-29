using application.EventHandlers.Interfaces;
using domain.Restaurants.Aggregates;
using FluentResults;
using infrastructure.Common;
using MassTransit;

namespace infrastructure.BusService.Emitters
{
    [EntityName("restaurants.changes")]
    internal class RestaurantChangedDto
    {
        public string Name { get; set; }
        internal RestaurantChangedDto(Restaurant restaurant)
        {
            Name = restaurant.Name.Value.Value;
        }
    }
    internal abstract class RestaurantEventEmitter : IEventEmitter<Restaurant>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        protected RestaurantEventEmitter(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

        public async Task<Result> EmitEvents(Restaurant @event, CancellationToken cancellationToken)
        {
            try
            {
                await _publishEndpoint.Publish(new RestaurantChangedDto(@event), cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex) {
                return Result.Fail(ex.Message);
            }
        }
    }
}
