﻿using application.EventHandlers.Interfaces;
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

            var policyResult = await FallbackRetryPoicies.AsyncRetry.ExecuteAndCaptureAsync(() =>
                _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.StreamId!.Value), cancellationToken));

            var isSuccess = policyResult.Outcome == OutcomeType.Successful;
            if(!isSuccess)
            {
                _logger.LogError("Consistency issue with {EventName} with {Id}", typeof(RestaurantCreatedEvent).Name, notification.StreamId);
            }
            await StoreEventAsync(notification, isSuccess, cancellationToken);
        }

        private static void Validation(RestaurantCreatedEvent notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notification.StreamId);
        }
        private async Task StoreEventAsync(RestaurantCreatedEvent notification, bool success, CancellationToken cancellationToken)
        {
            var eventStorageResult = await FallbackRetryPoicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StoreEventAsync(notification, success, cancellationToken));

            if (eventStorageResult.Outcome == OutcomeType.Failure)
            {
                _logger.LogError("Event storage issue with {EventName} with {Id}. {Data}", typeof(RestaurantCreatedEvent).Name, notification.StreamId, notification.Serialize());
            }
        }

    }
}
