using domain.Common.BaseTypes;

namespace domain.Common.DomainImplementationTypes.Identifiers
{
    public abstract class SimpleValueTypeId<TId> : ValueObject<TId>
        where TId : IComparable<TId>
    {
        protected SimpleValueTypeId(TId value)
        {
            Value = value;
        }

        public TId Value { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            SimpleValueTypeId<TId> other = obj as SimpleValueTypeId<TId>;
            if (other == null) return false;
            return Value.Equals(other.Value);
        }
    }
}
