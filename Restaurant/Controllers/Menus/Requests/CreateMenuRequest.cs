using application.Menus.Commands;
using domain.Menus.ValueObjects;

namespace webapi.Controllers.Menus.Requests;

public sealed class CreateMenuRequest
{
    public CreateMenuRequest(string? name, List<string> categories, List<CreateGroupRequest> groups, int restaurantId)
    {
        Name = name;
        Groups = groups;
        RestaurantId = restaurantId;
    }

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

        var categories = Categories.Select(c => new CreateCategoryCommand(c)).ToList();

        return new CreateMenuCommand(Name, categories,  groups, RestaurantId);
    }

    public string? Name { get; init; }
    public List<CreateGroupRequest> Groups { get; init; }
    public int RestaurantId { get; init; }
    public List<string> Categories { get; init; }
    
}

public record CreateGroupRequest
{
    public CreateGroupRequest(string? Name, List<CreateMealRequest> Meals)
    {
        this.Name = Name;
        this.Meals = Meals;
    }

    public string? Name { get; init; }
    public List<CreateMealRequest> Meals { get; init; }
}

public record CreateMealRequest
{
    public CreateMealRequest(string? Name, decimal Price, List<CreateIngredientRequest> Ingredients)
    {
        this.Name = Name;
        this.Price = Price;
        this.Ingredients = Ingredients;
    }

    public string? Name { get; init; }
    public decimal Price { get; init; }
    public List<CreateIngredientRequest> Ingredients { get; init; }
}

public record CreateIngredientRequest
{
    public CreateIngredientRequest(string? Name)
    {
        this.Name = Name;
    }

    public string? Name { get; init; }
}