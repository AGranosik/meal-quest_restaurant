using application.Menus.Commands;

namespace webapi.Controllers.Menus.Requests;

public record CreateMenuRequest
{
    public CreateMenuRequest(string? Name, List<CreateGroupRequest> Groups, int RestaurantId)
    {
        this.Name = Name;
        this.Groups = Groups;
        this.RestaurantId = RestaurantId;
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

        return new CreateMenuCommand(Name, groups, RestaurantId);
    }

    public string? Name { get; init; }
    public List<CreateGroupRequest> Groups { get; init; }
    public int RestaurantId { get; init; }

    public void Deconstruct(out string? Name, out List<CreateGroupRequest> Groups, out int RestaurantId)
    {
        Name = this.Name;
        Groups = this.Groups;
        RestaurantId = this.RestaurantId;
    }
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

    public void Deconstruct(out string? Name, out List<CreateMealRequest> Meals)
    {
        Name = this.Name;
        Meals = this.Meals;
    }
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

    public void Deconstruct(out string? Name, out decimal Price, out List<CreateIngredientRequest> Ingredients)
    {
        Name = this.Name;
        Price = this.Price;
        Ingredients = this.Ingredients;
    }
}

public record CreateIngredientRequest
{
    public CreateIngredientRequest(string? Name)
    {
        this.Name = Name;
    }

    public string? Name { get; init; }

    public void Deconstruct(out string? Name)
    {
        Name = this.Name;
    }
}