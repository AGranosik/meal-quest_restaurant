using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;

namespace application.EventHandlers.Interfaces
{
    public interface IEventInfoStorage<TAggregate, TKey>
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>
    {
        Task<int> StorePendingEventAsync(TAggregate notification, CancellationToken cancellationToken);
        Task StoreFailureAsync(int eventId, CancellationToken cancellationToken);
        Task StoreSuccessAsyncAsync(int eventId, CancellationToken cancellationToken);
    }
}
