using application.Menus.Commands;
using domain.Menus.ValueObjects;

namespace webapi.Controllers.Menus.Requests;

public sealed class CreateMenusRequest
{
    public List<CreateMenuRequest> MenusRequest { get; }

    public CreateMenusRequest(List<CreateMenuRequest> menusRequest)
    {
        MenusRequest = menusRequest;
    }

    public CreateMenusCommand CastToCommand()
    {
        var menus = new List<CreateMenuCommand>(MenusRequest.Count);
        foreach (var menuRequest in MenusRequest)
        {
            var groups = new List<CreateGroupCommand>(menuRequest.Groups.Count);
            foreach(var group in menuRequest.Groups)
            {
                var meals = new List<CreateMealCommand>(group.Meals.Count);

                foreach (var meal in group.Meals)
                {
                    //TODO: MAP METHODS ON EACH CLASS 
                    var categories = meal.Categories.Select(c => new CreateCategoryCommand(c)).ToList();
                    meals.Add(new CreateMealCommand(meal.Name, meal.Price,
                        meal.Ingredients.Select(i => new CreateIngredientCommand(i.Name)).ToList(),
                        categories));
                }

                var groupCommand = new CreateGroupCommand(group.Name, meals);
                groups.Add(groupCommand);
            }
            
            menus.Add(new CreateMenuCommand(menuRequest.Name,  groups, menuRequest.RestaurantId));
        }


        return new CreateMenusCommand(menus);
    }


    
}

public sealed class CreateMenuRequest
{
    public CreateMenuRequest(string? name, List<CreateGroupRequest> groups, int restaurantId)
    {
        Name = name;
        Groups = groups;
        RestaurantId = restaurantId;
    }
    
    public string? Name { get; init; }
    public List<CreateGroupRequest> Groups { get; init; }
    public int RestaurantId { get; init; }
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

public sealed class CreateMealRequest
{
    public CreateMealRequest(string? name, decimal price, List<CreateIngredientRequest> ingredients, List<string> categories)
    {
        Name = name;
        Price = price;
        Ingredients = ingredients;
        Categories = categories;
    }

    public string? Name { get; init; }
    public decimal Price { get; init; }
    public List<CreateIngredientRequest> Ingredients { get; init; }
    public List<string> Categories { get; init; }
}

public record CreateIngredientRequest
{
    public CreateIngredientRequest(string? Name)
    {
        this.Name = Name;
    }

    public string? Name { get; init; }
}