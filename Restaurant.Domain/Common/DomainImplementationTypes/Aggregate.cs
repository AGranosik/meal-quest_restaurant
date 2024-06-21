namespace Restaurant.Domain.Common.BaseTypes
{
    public abstract class Aggregate<TKey>(TKey id) : Entity<TKey>(id)
        where TKey : ValueObject<TKey>
    {
    }
}
