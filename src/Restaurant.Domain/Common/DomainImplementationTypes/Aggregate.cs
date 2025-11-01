namespace domain.Common.DomainImplementationTypes;

public abstract class Aggregate<TKey> : Entity<TKey>
    where TKey : ValueObject<TKey>
{
    protected Aggregate() : base() { }
}