using application.EventHandlers.Interfaces;
using domain.Menus.Aggregates;
using FluentResults;
using MassTransit;

namespace infrastructure.BusService.Emitters;

[EntityName("menus.changes")]
internal class MenuChangedDto
{
    public Menu Menu { get; init; }
    public int RestaurantId { get; set; }
    internal MenuChangedDto(Menu menu)
    {
        Menu =  menu;
        RestaurantId = menu.Restaurant.Value;
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
            await _publishEndpoint.Publish(new MenuChangedDto(@event), cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}