using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using MediatR;

namespace application.EventHandlers
{
    public record AggregateChangedEvent<TAggregate, TKey>(TAggregate Aggregate) : INotification
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>;

    public class AggregateChangedEventHandler<TAggregate, TKey> : INotificationHandler<AggregateChangedEvent<TAggregate, TKey>>
        where TKey : ValueObject<TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly IEventInfoStorage<TAggregate, TKey> _eventInfoStorage;

        public AggregateChangedEventHandler(IEventInfoStorage<TAggregate, TKey> eventInfoStorage)
        {
            _eventInfoStorage = eventInfoStorage;
        }

        public Task Handle(AggregateChangedEvent<TAggregate, TKey> notification, CancellationToken cancellationToken)
        {
            return null;
        }
    }
    //in futere metods like: before storage, after storage can be implmented

}
