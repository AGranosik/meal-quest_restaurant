using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
namespace infrastructure.EventStorage
{
    // tests
    internal class EventInfoStorage<TAggregate, TKey>(EventDbContext context) : IEventInfoStorage<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public Task StoreFilureAsnc(TKey aggregateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> StorePendingEventAsync(TKey notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StoreSuccessAsyncAsync(TKey aggregateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
