using application.EventHandlers.Interfaces;
using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage
{
    internal class EventInfoStorage<TDomainEvent>(EventDbContext context) : IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public Task StoreEventAsync(TDomainEvent @event, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
