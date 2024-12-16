using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using infrastructure.EventStorage.DatabaseModels;
using Microsoft.EntityFrameworkCore;
namespace infrastructure.EventStorage
{
    // TODO: retry should be implmeneted here?
    internal class EventInfoStorage<TAggregate, TKey>(EventDbContext context) : IEventInfoStorage<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private DomainEventModel<TAggregate, TKey>? _event;
        public async Task StoreFailureAsync(int eventId, CancellationToken cancellationToken)
        {
            var @event = await GetEventAsync(eventId, cancellationToken);
            @event.Failed();

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> StorePendingEventAsync(TAggregate notification, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TAggregate, TKey>();
            _event = DomainEventModel<TAggregate, TKey>.Pending(notification);

            dbSet.Add(_event);

            await _context.SaveChangesAsync(cancellationToken);
            return _event.EventId;
        }

        public async Task StoreSuccessAsyncAsync(int eventId, CancellationToken cancellationToken)
        {
            var @event = await GetEventAsync(eventId, cancellationToken);
            @event.Success();

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async ValueTask<DomainEventModel<TAggregate, TKey>> GetEventAsync(int eventId, CancellationToken cancellationToken)
        {
            if (_event is not null)
                return _event;

            var dbSet = _context.GetDbSet<TAggregate, TKey>();
            var @event = await dbSet.FirstOrDefaultAsync(e => e.EventId == eventId, cancellationToken);
            if(@event is null)
                throw new ArgumentException(nameof(@event));

            return @event;
        }
    }
}
