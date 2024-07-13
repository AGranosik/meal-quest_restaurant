using System.Runtime.CompilerServices;
using domain.Common.DomainImplementationTypes;

namespace domain.Common.BaseTypes
{
    public abstract class Aggregate<TKey> : Entity<TKey>
        where TKey : ValueObject<TKey>
    {

        protected Aggregate(TKey id) : base(id)
        {
            
        }

        protected List<DomainEvent<TKey>> _domainEvents;

        protected Aggregate() : base() { }
    }
}
