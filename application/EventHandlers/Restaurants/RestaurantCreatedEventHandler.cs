using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace application.EventHandlers.Restaurants
{
    public sealed class RestaurantCreatedEventHandler(IMenuRepository menuRepository, IEventInfoStorage<RestaurantEvent> eventInfoStorage, ILogger<RestaurantCreatedEventHandler> logger) : INotificationHandler<RestaurantCreatedEvent>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        private readonly IEventInfoStorage<RestaurantEvent> _eventInfoStorage = eventInfoStorage ?? throw new ArgumentNullException();
        private readonly ILogger<RestaurantCreatedEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            Validation(notification);

            await StoreAsPending(notification, cancellationToken);

            var policyResult = await FallbackRetryPoicies.AsyncRetry.ExecuteAndCaptureAsync(
                () => _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.StreamId!.Value), cancellationToken)
            );

            var isSuccess = policyResult.Outcome == OutcomeType.Successful;
            if(!isSuccess)
            {
                _logger.LogError("Consistency issue with {EventName} with {Id}", typeof(RestaurantCreatedEvent).Name, notification.StreamId);
            }
        }

        private static void Validation(RestaurantCreatedEvent notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notification.StreamId);
        }

        // store event first
        // emit with event id
        // make a pending status?? and change to it when events are queried
        // on emition store event as success if sth fail change to failed? but when there will be a problem with storage later it wil lstay as a success
        private async Task StoreAsPending(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventStorageResult = await FallbackRetryPoicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StorePendingEvent(notification, cancellationToken));

            if (eventStorageResult.Outcome == OutcomeType.Failure)
            {
                // add info why
                _logger.LogError("Event storage issue with {EventName} with {Id}. {Data}", typeof(RestaurantCreatedEvent).Name, notification.StreamId, notification.Serialize());
            }
        }

    }
}
