using application.EventHandlers.Interfaces;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using FluentResults;
using MassTransit;

namespace infrastructure.BusService.Emitters;
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

[EntityName("menus.changes")]
public sealed class MenuChangedDto
{
    public string Name { get; init; }
    public int RestaurantId { get; init; }
    public List<GroupDto> Groups { get; init; }

    internal MenuChangedDto(Menu menu)
    {
        RestaurantId = menu.Restaurant.Id!.Value;
        Name = menu.Name.Value.Value;
        Groups = menu.Groups
            .Select(g => new GroupDto(g.GroupName.Value.Value,
                g.Meals.Select(m => new MealDto(
                    m.Name!.Value.Value, m.Price!.Value,
                    m.Categories.Select(c => new CategoryDto(c.Id!.Value, c.Name.Value)).ToList(),
                    m.Ingredients!.Select(i => new IngredientDto(i.Name.Value)).ToList())).ToList())).ToList();
    }

}

public sealed record GroupDto(string Name, List<MealDto> Meals);
public sealed record MealDto(string Name, decimal Price, List<CategoryDto> Categories, List<IngredientDto> Ingredients);
public sealed record IngredientDto(string Name);
public sealed record CategoryDto(int Id, string Name);