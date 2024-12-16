using application.EventHandlers.Interfaces;
using domain.Restaurants.Aggregates;
using FluentResults;
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
    internal sealed class RestaurantEventEmitter(IPublishEndpoint publishEndpoint) : IEventEmitter<Restaurant>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task<Result> EmitEvents(Restaurant @event, CancellationToken cancellationToken)
        {
            try
            {
                using var rabbitTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(BusServiceConfiguration.TIMEOUT_LIMIT));
                using var mergedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, rabbitTimeout.Token);
                
                await _publishEndpoint.Publish(new RestaurantChangedDto(@event), mergedCancellationToken.Token);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
