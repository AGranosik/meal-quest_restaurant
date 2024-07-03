using core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Common.ValueTypes.Strings
{
    public class Name(string name) : ValueObject<Name>
    {
        public NotEmptyString Value { get; } = name;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Name? other = obj as Name;
            return other is not null && other.Value == Value;
        }
    }
}
