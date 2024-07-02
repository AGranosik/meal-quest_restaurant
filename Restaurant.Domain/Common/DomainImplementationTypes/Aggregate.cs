using Restaurant.Domain.Common.DomainImplementationTypes;

namespace Restaurant.Domain.Common.BaseTypes
{
    public abstract class Aggregate<TKey>(TKey id)
        where TKey : ValueObject<TKey>
    {
        protected List<DomainEvent<TKey>> _domainEvents;
        public TKey Id { get; } = id;
    }
}
