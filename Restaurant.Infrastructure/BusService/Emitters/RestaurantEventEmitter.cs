using application.EventHandlers.Interfaces;
using domain.Restaurants.Aggregates;
using FluentResults;
using MassTransit;

namespace infrastructure.BusService.Emitters;

[EntityName("restaurants.changes")]
public class RestaurantChangedDto
{
    public string Name { get; private set; }
    public double XAxis { get; private set; }
    public double YAxis { get; private set; }
    internal RestaurantChangedDto(Restaurant restaurant)
    {
        Name = restaurant.Name.Value.Value;
        XAxis = restaurant.Address.Coordinates.X;
        YAxis = restaurant.Address.Coordinates.Y;
    }
}
internal sealed class RestaurantEventEmitter : IEventEmitter<Restaurant>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RestaurantEventEmitter(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result> EmitEvents(Restaurant @event, CancellationToken cancellationToken)
    {
        try
        {
            using var rabbitTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(BusServiceConfiguration.TimeoutLimit));
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