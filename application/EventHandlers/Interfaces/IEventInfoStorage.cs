using domain.Common.DomainImplementationTypes;

namespace application.EventHandlers.Interfaces
{
    public interface IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        Task<int> StorePendingEvent(TDomainEvent @event, CancellationToken cancellationToken);
        Task StoreFilureAsnc(int EventId, CancellationToken cancellationToken);
        Task StoreSuccessAsync(int EventId, CancellationToken cancellationToken);
    }
}
