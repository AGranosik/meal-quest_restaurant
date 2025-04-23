using application.EventHandlers.Interfaces;
using domain.Menus.Aggregates;
using FluentResults;
using MassTransit;

namespace infrastructure.BusService.Emitters;

[EntityName("menus.changes")]
public class MenuChangedDto
{
    public Menu Menu { get; init; }
    public int RestaurantId { get; set; }
    internal MenuChangedDto(Menu menu)
    {
        Menu =  menu;
        RestaurantId = menu.Restaurant.Id!.Value;
    }
}

internal sealed class MenuEventEmitter : IEventEmitter<Menu>
{
    private readonly IPublishEndpoint _publishEndpoint;
    public MenuEventEmitter(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result> EmitEvents(Menu @event, CancellationToken cancellationToken)
    {
        try
        {
            //TODO: GENERIC
            using var rabbitTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(BusServiceConfiguration.TimeoutLimit));
            using var mergedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, rabbitTimeout.Token);
            await _publishEndpoint.Publish(new MenuChangedDto(@event), mergedCancellationToken.Token);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}