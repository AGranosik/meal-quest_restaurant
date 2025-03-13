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
    private Menu(List<Group> groups, Name name, RestaurantIdMenuId restaurant)
    {
        Groups = groups;
        Name = name;
        Restaurant = restaurant;
    }

    public static Result<Menu> Create(List<Group> groups,Name name, RestaurantIdMenuId restaurant)
    {
        var validatioNResult = CreationValidation(groups, restaurant);
        if (validatioNResult.IsFailed)
            return validatioNResult;

        var menu = new Menu(groups, name, restaurant);
        return Result.Ok(menu);
    }
    public Name Name { get; }
    public List<Group> Groups { get; }
    public RestaurantIdMenuId Restaurant { get; }

    private static Result CreationValidation(List<Group> groups, RestaurantIdMenuId restaurantId)
    {
        if (groups is null || groups.Count == 0)
            return Result.Fail("Groups are missing.");

        if (!groups.HasUniqueValues())
            return Result.Fail("Groups has to be unique.");

        if (restaurantId is null)
            return Result.Fail("Restaurant id cannot be null.");

        return Result.Ok();
    }
}