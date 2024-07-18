using domain.Common.BaseTypes;
using FluentResults;

namespace infrastructure.Database.Common
{
    public interface IAggregateRepository<TAggregate, TKey>
        where TAggregate : Aggregate<TKey>
        where TKey : ValueObject<TKey>
    {
        Task<Result> SaveAsync (TAggregate aggregate, CancellationToken cancellationToken);
    }
}
