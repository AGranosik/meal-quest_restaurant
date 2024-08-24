using application.EventHandlers.Interfaces;
using domain.Common.DomainImplementationTypes;
using infrastructure.EventStorage.DatabaseModels;

namespace infrastructure.EventStorage
{
    internal class EventInfoStorage<TDomainEvent>(EventDbContext context) : IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        // add reason of fialure;
        public async Task StoreEventAsync(TDomainEvent @event, bool success, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TDomainEvent>();

            dbSet.Add(new DomainEventModel<TDomainEvent>(@event.StreamId!.Value, @event, success));
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
