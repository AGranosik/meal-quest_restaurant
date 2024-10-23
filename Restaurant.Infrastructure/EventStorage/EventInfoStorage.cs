using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
namespace infrastructure.EventStorage
{
    // tests
    internal class EventInfoStorage<TAggregate, TKey>(EventDbContext context) : IEventInfoStorage<TAggregate, TKey>
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public Task StoreFilureAsnc(int aggregateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> StorePendingEvent(int @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StoreSuccessAsync(int aggregateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
