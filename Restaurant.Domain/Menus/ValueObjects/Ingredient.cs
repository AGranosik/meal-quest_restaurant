using core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Menus.ValueObjects
{
    public class Ingredient(NotEmptyString ingedientName) : ValueObject<Ingredient>, IEquatable<object?>
    {
        public NotEmptyString Name { get; } = ingedientName ?? throw new ArgumentNullException(nameof(ingedientName));

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
