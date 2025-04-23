using core.Exceptions;
using core.Extensions;
using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;

namespace domain.Menus.ValueObjects;

public sealed class Meal : ValueObject<Meal>
{
    public List<Ingredient>? Ingredients { get; }
    public Price? Price { get; }
    public Name? Name { get; }
    public List<Category> Categories { get; }

    public Meal(List<Ingredient> ingredients, List<Category> categories, Price price, Name name)
    {
        CreationValidation(ingredients, categories, price, name);
        Ingredients = ingredients;
        Price = price;
        Name = name;
        Categories = categories;
    }

    private Meal() { }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is not Meal other) return false;

        return Ingredients!.SequenceEqual(other.Ingredients!)
               && Price! == other.Price!
               && Name! == other.Name!;
    }

    private static void CreationValidation(List<Ingredient> ingredients, List<Category> categories, Price price, Name name)
    {
        ArgumentNullException.ThrowIfNull(price);
        ArgumentExceptionExtensions.ThrowIfNullOrEmpty(ingredients);
        if(!ingredients.HasUniqueValues())
            throw new ArgumentException("Not unique values for ingredients.");

        ArgumentExceptionExtensions.ThrowIfNullOrEmpty(categories);
        if(!categories.HasUniqueValues())
            throw new ArgumentException("Not unique values for categories.");
        
        ArgumentNullException.ThrowIfNull(name);
        
        
    }
}