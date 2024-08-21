using domain.Common.DomainImplementationTypes;

namespace application.EventHandlers.Interfaces
{
    public interface IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        Task StoreEventAsync(TDomainEvent @event, CancellationToken cancellationToken);
    }
}
