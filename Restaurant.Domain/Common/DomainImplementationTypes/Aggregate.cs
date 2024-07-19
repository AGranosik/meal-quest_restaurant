using domain.Common.DomainImplementationTypes;

namespace domain.Common.BaseTypes
{
    public abstract class Aggregate<TKey> : Entity<TKey>
        where TKey : ValueObject<TKey>
    {

        protected List<DomainEvent> _domainEvents;

        protected Aggregate() : base() { }
    }
}
