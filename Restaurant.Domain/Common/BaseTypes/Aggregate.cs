namespace Restaurant.Domain.Common.BaseTypes
{
    public class Aggregate<TKey>(TKey id) : Entity<TKey>(id)
        where TKey : ValueObject<TKey>
    {
    }
}
