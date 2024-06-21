using Restaurant.Core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Menus.ValueObjects
{
    public class Ingedient(NotEmptyString ingedientName) : ValueObject<Ingedient>
    {
        public NotEmptyString Name { get; } = ingedientName ?? throw new ArgumentNullException(nameof(ingedientName));

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Ingedient? other = obj as Ingedient;
            if (other == null) return false;
            return Name == other.Name;
        }
    }
}
