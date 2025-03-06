using application.Menus.Commands;

namespace webapi.Controllers.Menus.Requests;

public record CreateMenuRequest(string? Name, List<CreateGroupRequest> Groups, int RestaurantId)
{
    public CreateMenuCommand CastToCommand()
    {
        var groups = new List<CreateGroupCommand>(Groups.Count);
        foreach(var group in Groups)
        {
            var meals = new List<CreateMealCommand>(group.Meals.Count);

            foreach(var meal in group.Meals)
                meals.Add(new CreateMealCommand(meal.Name, meal.Price, meal.Ingredients.Select(i => new CreateIngredientCommand(i.Name)).ToList()));

            var groupCommand = new CreateGroupCommand(group.Name, meals);
            groups.Add(groupCommand);
        }

        return new CreateMenuCommand(Name, groups, RestaurantId);
    }
}

public record CreateGroupRequest(string? Name, List<CreateMealRequest> Meals);

public record CreateMealRequest(string? Name, decimal Price, List<CreateIngredientRequest> Ingredients);

public record CreateIngredientRequest(string? Name);