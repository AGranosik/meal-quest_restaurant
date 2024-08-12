using core.SimpleTypes;
using domain.Common.BaseTypes;
using FluentResults;

namespace domain.Menus.ValueObjects
{
    public class Ingredient: ValueObject<Ingredient>, IEquatable<object?>
    {
        public static Result<Ingredient> Create(NotEmptyString name)
        {
            return Result.Ok(new Ingredient(name));
        }

        protected Ingredient(NotEmptyString name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public NotEmptyString Name { get; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Ingredient? other = obj as Ingredient;
            if (other == null) return false;
            return Name == other.Name;
        }
    }
}
