using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;

namespace application.EventHandlers.Interfaces
{
    public interface IEventInfoStorage<TAggregate, TKey>
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>
    {
        Task<int> StorePendingEventAsync(TKey notification, CancellationToken cancellationToken);
        Task StoreFilureAsnc(TKey aggregateId, CancellationToken cancellationToken);
        Task StoreSuccessAsyncAsync(TKey aggregateId, CancellationToken cancellationToken);
    }
}
