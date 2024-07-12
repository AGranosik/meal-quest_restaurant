using domain.Common.DomainImplementationTypes;

namespace domain.Common.BaseTypes
{
    public abstract class Entity<TKey>
        where TKey : ValueObject<TKey>
    {
        protected List<DomainEvent<TKey>> _domainEvents = [];

        protected Entity() { }

        protected Entity(TKey id)
        {
            Id = id ?? throw new ArgumentNullException(typeof(TKey).Name);
        }

        public TKey Id { get; protected set; }

        public DomainEvent<TKey>[] GetEvents()
            => [.. _domainEvents];

        #region Equality
        public override bool Equals(object? obj)
        {
            var other = obj as Entity<TKey>;
            if (other is null)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
        {
            if (ReferenceEquals(a, b))
                return true;
            return a.Equals(b);
        }

        public static bool operator !=(Entity<TKey> a, Entity<TKey> b) => !(a == b);
        #endregion
    }
}
