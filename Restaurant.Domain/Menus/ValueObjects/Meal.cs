using Restaurant.Core.Exceptions;
using Restaurant.Core.Extensions;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Common.ValueTypes.Numeric;

namespace Restaurant.Domain.Menus.ValueObjects
{
    // should be different aggregate? or within restaurant
    // add menu (has resId) -> rest.addMenu(menuId) // but how to create it within single transaction? events
    // or
    // rest.add(menu) ??
    public class Meal : ValueObject<Meal>
    {
        public List<Ingredient> Ingredients { get; }
        public Price Price { get; }

        public Meal(List<Ingredient> ingredients, Price price)
        {
            CreationValidation(ingredients, price);
            Ingredients = ingredients;
            Price = price;
        }

        public override bool Equals(object? obj)
        {
            throw new NotImplementedException();
        }

        private static void CreationValidation(List<Ingredient> ingredients, Price price)
        {
            ArgumentNullException.ThrowIfNull(price);
            ArgumentExceptionExtensions.ThrowIfNullOrEmpty(ingredients);
            if(!ingredients.HasUniqueValues())
                throw new ArgumentException(nameof(ingredients));
        }
    }
}
