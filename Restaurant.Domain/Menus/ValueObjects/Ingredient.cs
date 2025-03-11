using core.SimpleTypes;
using domain.Common.DomainImplementationTypes;
using FluentResults;

namespace domain.Menus.ValueObjects;

public sealed class Ingredient: ValueObject<Ingredient>, IEquatable<object?>
{
    private Ingredient() { }
    public static Result<Ingredient> Create(NotEmptyString name)
    {
        return Result.Ok(new Ingredient(name));
    }

    private Ingredient(NotEmptyString name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
    public NotEmptyString Name { get; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is not Ingredient other) return false;
        return Name == other.Name;
    }
}