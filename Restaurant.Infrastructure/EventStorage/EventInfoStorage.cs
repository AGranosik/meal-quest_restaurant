using application.EventHandlers.Interfaces;
using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage
{
    internal class EventInfoStorage<TDomainEvent> : IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        public Task StoreEventAsync(TDomainEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
