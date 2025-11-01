using application.Menus.Commands;
using domain.Menus.ValueObjects;

namespace webapi.Controllers.Menus.Requests;

public sealed class CreateMenuRequest
{
    public CreateMenuRequest(string? name, List<CreateGroupRequest> groups, int restaurantId, bool isActive)
    {
        Name = name;
        Groups = groups;
        RestaurantId = restaurantId;
        IsActive = isActive;
    }

    public string? Name { get; }
    public List<CreateGroupRequest> Groups { get; }
    public int RestaurantId { get; }
    public bool IsActive { get; }

    public CreateMenuCommand CastToCommand()
    {
        var groups = new List<CreateGroupCommand>(Groups.Count);
        groups.AddRange(Groups.Select(group => group.CastToCommand()));
        return new CreateMenuCommand(Name, groups, RestaurantId, IsActive);
    }
}

public record CreateGroupRequest
{
    public CreateGroupRequest(string? Name, List<CreateMealRequest> Meals)
    {
        this.Name = Name;
        this.Meals = Meals;
    }

    public string? Name { get; }
    public List<CreateMealRequest> Meals { get; }

    public CreateGroupCommand CastToCommand()
    {
        var meals = new List<CreateMealCommand>(Meals.Count);
        meals.AddRange(Meals.Select(meal => meal.CastToCommand()));

        var groupCommand = new CreateGroupCommand(Name, meals);
        return groupCommand;
    }
}

public sealed class CreateMealRequest
{
    public CreateMealRequest(string? name, decimal price, List<CreateIngredientRequest> ingredients, List<string> categories)
    {
        Name = name;
        Price = price;
        Ingredients = ingredients;
        Categories = categories;
    }

    public string? Name { get; }
    public decimal Price { get; }
    public List<CreateIngredientRequest> Ingredients { get; }
    public List<string> Categories { get; }

    public CreateMealCommand CastToCommand()
    {
        var categories = Categories.Select(c => new CreateCategoryCommand(c)).ToList();
        return new CreateMealCommand(Name, Price,
            Ingredients.Select(i => new CreateIngredientCommand(i.Name)).ToList(),
            categories);
    }
}

public record CreateIngredientRequest
{
    public CreateIngredientRequest(string? Name)
    {
        this.Name = Name;
    }

    public string? Name { get; }
}