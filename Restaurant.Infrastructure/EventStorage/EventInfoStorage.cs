using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using infrastructure.EventStorage.DatabaseModels;
namespace infrastructure.EventStorage
{
    // TODO: TESTS
    public class EventInfoStorage<TAggregate, TKey>(EventDbContext context) : IEventInfoStorage<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        //TODO: store event reference within a scope
        public Task StoreFailureAsync(int eventId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> StorePendingEventAsync(TAggregate notification, CancellationToken cancellationToken)
        {
            var dbSet = _context.GetDbSet<TAggregate, TKey>();
            var @event = DomainEventModel<TAggregate, TKey>.Pending(notification);

            dbSet.Add(@event);

            await _context.SaveChangesAsync(cancellationToken);
            return @event.EventId;
        }

        public Task StoreSuccessAsyncAsync(int eventId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
