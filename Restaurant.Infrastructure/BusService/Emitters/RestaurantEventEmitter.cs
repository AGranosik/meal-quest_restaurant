using application.EventHandlers.Interfaces;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects;
using FluentResults;
using MassTransit;

namespace infrastructure.BusService.Emitters;

[EntityName("restaurants.changes")]
public sealed class RestaurantChangedDto
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public byte[]? LogoData { get; private set; }
    public AddressQueueDto Address { get; set; }

    internal RestaurantChangedDto(Restaurant restaurant)
    {
        Name = restaurant.Name.Value.Value;
        Description = restaurant.Description.Value.Value;
        LogoData = restaurant.Logo?.Data;
        Address = new AddressQueueDto(restaurant.Address);
    }
}

public sealed class AddressQueueDto
{
    public string StreetName { get; private set; }
    public string City { get; private set; }
    public double XAxis { get; private set; }
    public double YAxis { get; private set; }

    internal AddressQueueDto(Address address)
    {
        XAxis = address.Coordinates.X;
        YAxis = address.Coordinates.Y;
        StreetName = address.Street.Value.Value;
        City = address.City.Value.Value;
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