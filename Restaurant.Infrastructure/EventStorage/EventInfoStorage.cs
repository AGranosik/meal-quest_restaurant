using application.EventHandlers.Interfaces;
using domain.Common.DomainImplementationTypes;
using infrastructure.EventStorage.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.EventStorage
{
    // tests
    internal class EventInfoStorage<TDomainEvent>(EventDbContext context) : IEventInfoStorage<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task StoreFilureAsnc(int EventId, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TDomainEvent>();
            await dbSet.Where(e => e.EventId == EventId)
                .ExecuteUpdateAsync(
                    db => db.SetProperty(e => e.PropgationStatus, EventProapgationStatus.Failed), cancellationToken
                );
        }

        public async Task<int> StorePendingEvent(TDomainEvent @event, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TDomainEvent>();
            var model = DomainEventModel<TDomainEvent>.Pending(@event.StreamId!.Value, @event);
            dbSet.Add(model);
            await _context.SaveChangesAsync(cancellationToken);

            return model.EventId;
        }

        public async Task StoreSuccessAsync(int EventId, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TDomainEvent>();
            await dbSet.Where(e => e.EventId == EventId)
                .ExecuteUpdateAsync(
                    db => db.SetProperty(e => e.PropgationStatus, EventProapgationStatus.Propagated), cancellationToken
                );
        }
    }
}
