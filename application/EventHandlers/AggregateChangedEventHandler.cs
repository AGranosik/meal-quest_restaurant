using System.Text.Json;
using application.EventHandlers.Interfaces;
using core.FallbackPolicies;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using domain.Restaurants.Aggregates;
using MassTransit;
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
        protected readonly IEventEmitter<TAggregate> _eventEmitter;

        protected AggregateChangedEventHandler(IEventInfoStorage<TAggregate, TKey> eventInfoStorage, ILogger<AggregateChangedEventHandler<TAggregate, TKey>> logger, IEventEmitter<TAggregate> eventEmitter)
        {
            _eventInfoStorage = eventInfoStorage;
            _logger = logger;
            _eventEmitter = eventEmitter;
        }

        public async Task Handle(AggregateChangedEvent<TAggregate, TKey> notification, CancellationToken cancellationToken)
        {
            Validation(notification);
            var policyResult = await FallbackRetryPolicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StorePendingEventAsync(notification.Aggregate, cancellationToken));

            var storedSuccessfully = policyResult.Outcome == OutcomeType.Successful;

            if (!storedSuccessfully)
                _logger.LogError(policyResult.FinalException, "Problem with storage {AggreateName} Id: {Id}", typeof(TAggregate).Name, notification.Aggregate.Id!.Value);

            var result = await ProcessingEventAsync(notification, cancellationToken);
            if (result && storedSuccessfully)
                await ProcessSuccessAsync(notification.Aggregate.Id!, policyResult.Result, cancellationToken);

            if (!result && storedSuccessfully)
                await ProcessFailureAsync(notification.Aggregate.Id!, policyResult.Result, cancellationToken);
        }

        private async Task ProcessFailureAsync(TKey notificationId, int eventId, CancellationToken cancellationToken)
        {
            var policyResult = await FallbackRetryPolicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StoreFailureAsync(eventId, cancellationToken));

            if (policyResult.Outcome == OutcomeType.Failure)
                _logger.LogError(policyResult.FinalException, "Problem with failure storage. {AggregateName}, Id: {Id}", typeof(TAggregate).Name, notificationId);
        }

        private async Task ProcessSuccessAsync(TKey notificationId, int eventId, CancellationToken cancellationToken)
        {
            var policyResult = await FallbackRetryPolicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StoreSuccessAsyncAsync(eventId, cancellationToken));

            if (policyResult.Outcome == OutcomeType.Failure)
                _logger.LogError(policyResult.FinalException, "Problem with success storage. {AggregateName}, Id: {Id}", typeof(TAggregate).Name, notificationId);
        }

        protected abstract Task<bool> ProcessingEventAsync(AggregateChangedEvent<TAggregate, TKey> notification, CancellationToken cancellationToken);
        private static void Validation(AggregateChangedEvent<TAggregate, TKey> notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notification.Aggregate);
            ArgumentNullException.ThrowIfNull(notification.Aggregate.Id);
        }
    }

}
