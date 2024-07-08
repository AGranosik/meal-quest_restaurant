using domain.Common.BaseTypes;

namespace domain.Common.DomainImplementationTypes.Identifiers
{
    public abstract class SimpleValueType<TValue, TKey> : ValueObject<TKey>
    {
        protected SimpleValueType(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            SimpleValueType<TValue, TKey> other = obj as SimpleValueType<TValue, TKey>;
            if (other == null) return false;
            return Value.Equals(other.Value);
        }
    }
}
