using FluentResults;
using core.Extensions;
using domain.Menus.ValueObjects.Identifiers;
using domain.Menus.ValueObjects;
using domain.Common.ValueTypes.Strings;
using System.Text.Json.Serialization;
using domain.Common.DomainImplementationTypes;

namespace domain.Menus.Aggregates;

public sealed class Menu : Aggregate<MenuId>
{
    [JsonConstructor]
    private Menu() { }
    private Menu(List<Group> groups, Name name, MenuRestaurant restaurant, bool isActive)
    {
        Groups = groups;
        Name = name;
        Restaurant = restaurant;
        IsActive = isActive;
    }

    public static Result<Menu> Create(List<Group> groups,Name name, MenuRestaurant? restaurant, bool isActive)
    {
        var validationResult = CreationValidation(groups, restaurant);
        if (validationResult.IsFailed)
            return validationResult;

        var menu = new Menu(groups, name, restaurant!, isActive);
        return Result.Ok(menu);
    }
    public Name Name { get; }
    public List<Group> Groups { get; }
    public MenuRestaurant Restaurant { get; }
    public bool IsActive { get; }

    private static Result CreationValidation(List<Group> groups, MenuRestaurant? restaurantId)
    {
        if (groups.Count == 0)
            return Result.Fail("Groups are missing.");

        if (!groups.HasUniqueValues())
            return Result.Fail("Groups has to be unique.");

        return restaurantId is null ? Result.Fail("Restaurant id cannot be null.") : Result.Ok();
    }
}