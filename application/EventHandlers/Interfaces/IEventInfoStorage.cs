using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;

namespace application.EventHandlers.Interfaces
{
    public interface IEventInfoStorage<TAggregate, TKey>
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>
    {
        Task<int> StorePendingEvent(int @event, CancellationToken cancellationToken);
        Task StoreFilureAsnc(int aggregateId, CancellationToken cancellationToken);
        Task StoreSuccessAsync(int aggregateId, CancellationToken cancellationToken);
    }
}
