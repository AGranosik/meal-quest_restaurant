using application.Menus.Commands.Interfaces;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using MediatR;

namespace application.Menus.Commands
{
    public class CreateMenuCommandHandler(IMenuRepository menuRepository) : IRequestHandler<CreateMenuCommand, Result<MenuId>>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));

        public async Task<Result<MenuId>> Handle(CreateMenuCommand command, CancellationToken cancellationToken)
        {
            var validationResult = Validation(command);
            if (validationResult.IsFailed)
                return validationResult;


            throw new NotImplementedException();
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

        //private static Result<Menu> CreateDomainModel(CreateMenuCommand command)
        //{
        //    var ingriedients = command.Groups?.Select(
        //        g => Group.Create(
        //            g?.Meals.Select(m => new Meal(m?.Ingredients.Select(i => Ingredient.Create(i.Name)))
        //        ));
        //    var meal = new Meal();
        //    var groups = Group.Create();
        //    var menu = Menu.Create();
        //}
    }

    public record CreateMenuCommand(string? Name, List<CreateGroupCommand> Groups) : IRequest<Result<MenuId>>;
    public record CreateGroupCommand(string? GroupName, List<CreateMealCommand> Meals);
    public record CreateMealCommand(string? Name, decimal Price, List<CreateIngredientCommand> Ingredients);
    public record CreateIngredientCommand(string? Name);
}
