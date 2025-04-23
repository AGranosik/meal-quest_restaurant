using application.Common.Extensions;
using application.Menus.Commands.Interfaces;
using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace application.Menus.Commands;

internal sealed class CreateMenuCommandHandler : IRequestHandler<CreateMenusCommand, Result<List<MenuId>>>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateMenuCommandHandler> _logger;

    public CreateMenuCommandHandler(IMenuRepository menuRepository, IMediator mediator,
        ILogger<CreateMenuCommandHandler> logger)
    {
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<List<MenuId>>> Handle(CreateMenusCommand menusCommand, CancellationToken cancellationToken)
    {
        var validationResult = Validation(menusCommand);
        if (validationResult.IsFailed)
            return validationResult;
        var domainResult = CreateDomainModel(menusCommand.MenusCommand);
        if (domainResult.IsFailed)
            return domainResult.ToResult();

        await _menuRepository.CreateMenuAsync(domainResult.Value, cancellationToken);
        await _mediator.PublishEventsAsync<Menu, MenuId>(domainResult.Value, _logger, cancellationToken);
        return domainResult.Value.Select(d => d.Id!).ToList();
    }

    private static Result Validation(CreateMenusCommand? menusCommand)
    {
        if (menusCommand == null)
            return Result.Fail("Command cannot be null.");
        var command = menusCommand.MenusCommand;
        if (command is null || command.Count == 0)
            return Result.Fail("Command cannot be null.");

        foreach (var menuCommand in command)
        {
            var groups = menuCommand.Groups;
            if (!groups.Any())
                return Result.Fail("Groups cannot be null.");


            var meals = groups.SelectMany(g => g.Meals).ToList();
            if (meals.Count == 0)
                return Result.Fail("Meals cannot be null.");


            var ingredients = meals.SelectMany(m => m.Ingredients).ToList();
            if (ingredients.Count == 0)
                return Result.Fail("Ingredients cannot be null.");

            var categories = meals.SelectMany(m => m.Categories).ToList();
            if (categories.Count == 0)
                return Result.Fail("Categories cannot be null.");

            if (string.IsNullOrEmpty(menuCommand.Name))
                return Result.Fail("Menu name cannot be empty.");
        }

        return Result.Ok();
    }

    private static Result<List<Menu>> CreateDomainModel(List<CreateMenuCommand> command)
    {
        //SAME CATEGORY REFERENCE
        var commandGroups = command.SelectMany(c => c.Groups).ToList();
        var groups = new List<Group>(commandGroups.Count);
        var uniqueCategories = commandGroups.SelectMany(g => g.Meals)
            .SelectMany(m => m.Categories).Select(c => c.Name).Distinct()
            .Select(c => new Category(c))
            .ToList();

        var menus = new List<Menu>(command.Count);

        foreach (var menuCommand in command)
        {
            foreach (var commandGroup in menuCommand.Groups)
            {
                var meals = new List<Meal>(commandGroup.Meals.Count);

                foreach (var commandMeal in commandGroup.Meals)
                {
                    var ingredients = new List<Ingredient>(commandMeal.Ingredients.Count);
                    var categories =
                        uniqueCategories.Where(uq => commandMeal.Categories.Any(c => c.Name == uq.Name.Value))
                            .ToList();

                    foreach (var domainIngredient in commandMeal.Ingredients.Select(commandIngredient =>
                                 Ingredient.Create(commandIngredient.Name!)))
                    {
                        if (domainIngredient.IsFailed)
                            return domainIngredient.ToResult();
                        ingredients.Add(domainIngredient.Value);
                    }

                    meals.Add(new Meal(ingredients, categories, new Price(commandMeal.Price),
                        new Name(commandMeal.Name!)));
                }

                var group = Group.Create(meals, new Name(commandGroup.GroupName!));
                if (group.IsFailed)
                    return group.ToResult();

                groups.Add(group.Value);
            }

            var menu = Menu.Create(groups, new Name(menuCommand.Name!),
                new MenuRestaurant(new RestaurantIdMenuId(menuCommand.RestaurantId)));
            if (menu.IsFailed)
                return Result.Fail(menu.Errors);

            menus.Add(menu.Value);
        }

        return Result.Ok(menus);
    }
}

public sealed class CreateMenusCommand : IRequest<Result<List<MenuId>>>
{
    public CreateMenusCommand(List<CreateMenuCommand> menusCommand)
    {
        MenusCommand = menusCommand ?? [];
    }

    public List<CreateMenuCommand> MenusCommand { get; init; }
}

public sealed class CreateMenuCommand
{
    public CreateMenuCommand(string? name, List<CreateGroupCommand> groups, int restaurantId)
    {
        Name = name;
        Groups = groups ?? [];
        RestaurantId = restaurantId;
    }

    public string? Name { get; init; }
    public List<CreateGroupCommand> Groups { get; init; }
    public int RestaurantId { get; init; }
}

public record CreateGroupCommand
{
    public CreateGroupCommand(string? GroupName, List<CreateMealCommand> Meals)
    {
        this.GroupName = GroupName;
        this.Meals = Meals ?? [];
    }

    public string? GroupName { get; init; }
    public List<CreateMealCommand> Meals { get; init; }
}

public sealed class CreateMealCommand
{
    public string? Name { get; }
    public decimal Price { get; }
    public List<CreateIngredientCommand> Ingredients { get; }
    public List<CreateCategoryCommand> Categories { get; }

    public CreateMealCommand(string? name, decimal price, List<CreateIngredientCommand> ingredients,
        List<CreateCategoryCommand> categories)
    {
        Name = name;
        Price = price;
        Ingredients = ingredients ?? [];
        Categories = categories ?? [];
    }
}

public sealed class CreateIngredientCommand
{
    public string? Name { get; }

    public CreateIngredientCommand(string? name)
    {
        Name = name;
    }
}

public sealed class CreateCategoryCommand
{
    public string Name { get; }

    public CreateCategoryCommand(string name)
    {
        Name = name;
    }
}