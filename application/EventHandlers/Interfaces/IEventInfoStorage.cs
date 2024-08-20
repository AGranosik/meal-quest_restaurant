using domain.Common.DomainImplementationTypes;

namespace application.EventHandlers.Interfaces
{
    public interface IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        Task<int> StoreEventAsync(TDomainEvent @event, CancellationToken cancellationToken);
        Task MarkAsNotSentAsync(int eventId, CancellationToken cancellationToken);
        Task MarkAsSentAsync(int eventId, CancellationToken cancellationToken);
    }
}
