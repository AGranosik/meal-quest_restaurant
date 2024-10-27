using System.Text.Json.Serialization;
using domain.Common.BaseTypes;

namespace domain.Common.DomainImplementationTypes.Identifiers
{
    public abstract class SimpleValueType<TValue, TKey>: ValueObject<TKey>
    {
        public TValue Value { get; }
        protected SimpleValueType(TValue value)
        {
            Value = value;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj is not SimpleValueType<TValue, TKey> other) return false;
            return Value!.Equals(other.Value);
        }
    }
}
