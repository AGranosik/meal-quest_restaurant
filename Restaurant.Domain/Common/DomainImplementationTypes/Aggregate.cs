using domain.Common.DomainImplementationTypes;

namespace domain.Common.BaseTypes
{
    public abstract class Aggregate<TKey>(TKey id)
        where TKey : ValueObject<TKey>
    {
        protected List<DomainEvent<TKey>> _domainEvents;
        public TKey Id { get; } = id ?? throw new ArgumentNullException(nameof(id));
    }
}
