using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using MediatR;

namespace application.EventHandlers
{
    public record AggregateChangedEvent<TAggregate, TKey>(TAggregate Aggregate) : INotification
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>;

    public abstract class AggregateChangedEventHandler<TAggregate, TKey> : INotificationHandler<AggregateChangedEvent<TAggregate, TKey>>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        private readonly IEventInfoStorage<TAggregate, TKey> _eventInfoStorage;

        public AggregateChangedEventHandler(IEventInfoStorage<TAggregate, TKey> eventInfoStorage)
        {
            _eventInfoStorage = eventInfoStorage;
        }

        public async Task Handle(AggregateChangedEvent<TAggregate, TKey> notification, CancellationToken cancellationToken)
        {
            Validation(notification);
            await _eventInfoStorage.StorePendingEventAsync(notification.Aggregate.Id!, cancellationToken);
        }

        protected abstract Task ProcessingEventAsync(AggregateChangedEvent<TAggregate, TKey> notification, CancellationToken cancellationToken);

        private void Validation(AggregateChangedEvent<TAggregate, TKey> notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notification.Aggregate);
            ArgumentNullException.ThrowIfNull(notification.Aggregate.Id);
        } 
    }

}
