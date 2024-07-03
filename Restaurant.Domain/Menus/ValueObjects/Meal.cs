using core.Exceptions;
using core.Extensions;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Common.ValueTypes.Strings;

namespace Restaurant.Domain.Menus.ValueObjects
{
    // size or weight of meal
    public class Meal : ValueObject<Meal>
    {
        public List<Ingredient> Ingredients { get; }
        public Price Price { get; }
        public Name Name { get; set; }

        public Meal(List<Ingredient> ingredients, Price price, Name name)
        {
            CreationValidation(ingredients, price, name);
            Ingredients = ingredients;
            Price = price;
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Meal? other = obj as Meal;
            if (other == null) return false;

            return Ingredients.SequenceEqual(other.Ingredients)
                && Price == other.Price
                && Name == other.Name;
        }

        private static void CreationValidation(List<Ingredient> ingredients, Price price, Name name)
        {
            ArgumentNullException.ThrowIfNull(price);
            ArgumentExceptionExtensions.ThrowIfNullOrEmpty(ingredients);
            if(!ingredients.HasUniqueValues())
                throw new ArgumentException(nameof(ingredients));

            ArgumentNullException.ThrowIfNull(name);
        }
    }
}
