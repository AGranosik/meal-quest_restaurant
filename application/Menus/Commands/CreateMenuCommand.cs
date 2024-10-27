using application.Common.Extensions;
using application.Menus.Commands.Interfaces;
using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace application.Menus.Commands
{
    // remove uneccessary logs for metrics
    public sealed class CreateMenuCommandHandler(IMenuRepository menuRepository, IMediator mediator, ILogger<CreateMenuCommandHandler> logger) : IRequestHandler<CreateMenuCommand, Result<MenuId>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly ILogger<CreateMenuCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<Result<MenuId>> Handle(CreateMenuCommand command, CancellationToken cancellationToken)
        {
            var validationResult = Validation(command);
            if (validationResult.IsFailed)
                return validationResult;

            var domainResult = CreateDomainModel(command);
            if (domainResult.IsFailed)
                return domainResult.ToResult();

            await _menuRepository.CreateMenuAsync(domainResult.Value, cancellationToken);
            await _mediator.PublishEventsAsync<Menu, MenuId>(domainResult.Value, _logger, cancellationToken);
            return Result.Ok(domainResult.Value.Id!);
        }

        private static Result Validation(CreateMenuCommand command)
        {
            if (command is null)
                return Result.Fail("Command cannot be null.");

            var groups = command.Groups;
            if (groups is null || !groups.Any())
                return Result.Fail("Groups cannot be null.");

            var meals = groups.Where(g => g.Meals is not null).SelectMany(g => g.Meals).ToList();
            if (meals.Count == 0)
                return Result.Fail("Meals cannot be null.");

            var ingredients = meals.Where(m => m.Ingredients is not null).SelectMany(m => m.Ingredients).ToList();
            if (ingredients.Count == 0)
                return Result.Fail("Ingredients cannot be null.");

            return Result.Ok();
        }

        private static Result<Menu> CreateDomainModel(CreateMenuCommand command)
        {
            var groups = new List<Group>(command.Groups.Count);

            foreach (var commandGroup in command.Groups)
            {
                var meals = new List<Meal>(commandGroup.Meals.Count);

                foreach(var commandMeal in commandGroup.Meals)
                {
                    var ingriedients = new List<Ingredient>(commandMeal.Ingredients.Count);

                    foreach(var commandIngredient in commandMeal.Ingredients)
                    {
                        var domainIngredient = Ingredient.Create(commandIngredient.Name!);
                        if (domainIngredient.IsFailed)
                            return domainIngredient.ToResult();
                        ingriedients.Add(domainIngredient.Value);
                    }
                    meals.Add(new Meal(ingriedients, new Price(commandMeal.Price), new Name(commandMeal.Name!)));
                }
                var group = Group.Create(meals, new Name(commandGroup.GroupName!));
                if(group.IsFailed)
                    return group.ToResult();

                groups.Add(group.Value);
            }
            var menu = Menu.Create(groups, new Name(command.Name!), new RestaurantIdMenuId(command.RestaurantId));
            if (menu.IsFailed)
                return menu.ToResult();

            return menu.Value;
        }
    }

    public record CreateMenuCommand(string? Name, List<CreateGroupCommand> Groups, int RestaurantId) : IRequest<Result<MenuId>>;
    public record CreateGroupCommand(string? GroupName, List<CreateMealCommand> Meals);
    public record CreateMealCommand(string? Name, decimal Price, List<CreateIngredientCommand> Ingredients);
    public record CreateIngredientCommand(string? Name);
}
