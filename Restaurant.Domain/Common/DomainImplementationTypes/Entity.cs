using domain.Common.DomainImplementationTypes;

namespace domain.Common.BaseTypes
{
    public abstract class Entity<TKey>
        where TKey : ValueObject<TKey>
    {
        protected List<DomainEvent> _domainEvents = [];

        protected Entity() { }

        public TKey? Id { get; protected set; }

        public void SetId(TKey id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        #region Equality
        public override bool Equals(object? obj)
        {
            var other = obj as Entity<TKey>;
            if (other is null)
                return false;

            if (Id is null || other.Id is null)
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
