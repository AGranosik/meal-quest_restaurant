﻿using core.Exceptions;
using core.Extensions;
using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;

namespace domain.Menus.ValueObjects
{
    public class Meal : ValueObject<Meal>
    {
        public List<Ingredient> Ingredients { get; }
        public Price Price { get; }
        public Name Name { get; }

        public Meal(List<Ingredient> ingredients, Price price, Name name)
        {
            CreationValidation(ingredients, price, name);
            Ingredients = ingredients;
            Price = price;
            Name = name;
        }

        protected Meal() { }

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
