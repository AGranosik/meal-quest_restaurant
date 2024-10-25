using application.EventHandlers.Interfaces;
using core.FallbackPolicies;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace application.EventHandlers
{
    public record AggregateChangedEvent<TAggregate, TKey>(TAggregate Aggregate) : INotification
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>;

    public abstract class AggregateChangedEventHandler<TAggregate, TKey> : INotificationHandler<AggregateChangedEvent<TAggregate, TKey>>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        protected readonly IEventInfoStorage<TAggregate, TKey> _eventInfoStorage;
        protected readonly ILogger<AggregateChangedEventHandler<TAggregate, TKey>> _logger;

        public AggregateChangedEventHandler(IEventInfoStorage<TAggregate, TKey> eventInfoStorage, ILogger<AggregateChangedEventHandler<TAggregate, TKey>> logger)
        {
            _eventInfoStorage = eventInfoStorage;
            _logger = logger;
        }

        public async Task Handle(AggregateChangedEvent<TAggregate, TKey> notification, CancellationToken cancellationToken)
        {
            Validation(notification);
            var policyResult = await FallbackRetryPoicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StorePendingEventAsync(notification.Aggregate.Id!, cancellationToken));

            if (policyResult.Outcome == OutcomeType.Failure)
                _logger.LogError("Problem with {aggreateName} Id: {Id}", typeof(TAggregate).Name, notification.Aggregate.Id!.Value);

            await ProcessingEventAsync(notification, cancellationToken);
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
