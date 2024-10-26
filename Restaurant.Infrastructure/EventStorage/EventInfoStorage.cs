using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
namespace infrastructure.EventStorage
{
    // TODO: TESTS
    internal class EventInfoStorage<TAggregate, TKey>(EventDbContext context) : IEventInfoStorage<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly EventDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        //TODO: store event reference within a scope
        public Task StoreFailureAsync(int eventId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> StorePendingEventAsync(TKey notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StoreSuccessAsyncAsync(int eventId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
