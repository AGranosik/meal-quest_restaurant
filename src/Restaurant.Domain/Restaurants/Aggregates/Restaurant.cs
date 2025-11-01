using System.Text.Json.Serialization;
using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates;

public sealed class Restaurant: Aggregate<RestaurantId>
{
    private readonly List<Menu> _menus = [];
    public IReadOnlyCollection<Menu> Menus => _menus.AsReadOnly();

    public Name Name { get; }
    public Owner Owner { get; }
    public OpeningHours OpeningHours { get; }
    public Address Address { get; }
    public Description Description { get; }
    public RestaurantLogo? Logo { get; set; }
    public static Result<Restaurant> Create(Name name, Owner owner, OpeningHours openingHours, Address address, Description description, RestaurantLogo? logo)
    {
        var creationResult = CreationValidation(name, owner, openingHours, address, description);
        return creationResult.IsFailed ? creationResult : Result.Ok(new Restaurant(name, owner, openingHours, address, description, logo));
    }

    public Result AddMenu(Menu menu)
    {
        if (_menus.Contains(menu))
            return Result.Fail("Menu already at restaurant.");
        
        _menus.Add(menu);
        return Result.Ok();
    }

    [JsonConstructor]
    private Restaurant() : base() { }

    private Restaurant(Name name, Owner owner, OpeningHours openingHours, Address address, Description description, RestaurantLogo? logo)
    {
        Owner = owner;
        OpeningHours = openingHours;
        Name = name;
        Address = address;
        Description = description;
        Logo = logo;
    }

    private static Result CreationValidation(Name name, Owner owner, OpeningHours openingHours, Address address, Description description)
    {
        if (name is null)
            return Result.Fail("Name cannot be null.");

        if (owner is null)
            return Result.Fail("Owner cannot be null.");

        if (openingHours is null)
            return Result.Fail("Opening houts cannot be null.");

        if (address is null)
            return Result.Fail("Address cannot be null.");
        
        if(description is null)
            return Result.Fail("Description cannot be null.");

        return Result.Ok();
    }
}