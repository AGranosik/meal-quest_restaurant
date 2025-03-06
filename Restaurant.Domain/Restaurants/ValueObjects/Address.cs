using domain.Common.BaseTypes;
using FluentResults;

namespace domain.Restaurants.ValueObjects;

public class Address : ValueObject<Address>
{

    public static Result<Address> Create(Street street, City city, Coordinates coordinates)
    {
        var validationResult = Validation(street, city, coordinates);
        if (validationResult.IsFailed)
            return validationResult;

        return Result.Ok(new Address(street, city, coordinates));
    }

    protected Address(Street street, City city, Coordinates coordinates)
    {
        Street = street;
        City = city;
        Coordinates = coordinates;
    }

    private Address() { }

    public Street Street { get; }
    public City City { get; }
    public Coordinates Coordinates { get; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is not Address other) return false;
        return Street! == other.Street! && City! == other.City! && Coordinates! == other.Coordinates!;
    }

    private static Result Validation(Street street, City city, Coordinates coordinates)
    {
        if (street is null)
            return Result.Fail("Street cannot be empty.");

        if (city is null)
            return Result.Fail("City cannot be empty.");

        if (coordinates is null)
            return Result.Fail("Coordinates cannot be empty.");

        return Result.Ok();
    }
}