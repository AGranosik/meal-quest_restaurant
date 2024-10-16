using application.EventHandlers.Interfaces;
using application.Restaurants.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.DomainEvents;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace application.EventHandlers.Menus
{
    public sealed class MenuCreatedEventHandler(IRestaurantRepository restaurantRepository, IEventInfoStorage<MenuEvent> eventInfoStorage, ILogger<MenuCreatedEventHandler> logger) : INotificationHandler<MenuCreatedEvent>
    {
        private readonly IRestaurantRepository _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
        private readonly IEventInfoStorage<MenuEvent> _eventInfoStorage = eventInfoStorage ?? throw new ArgumentNullException(nameof(eventInfoStorage));
        private readonly ILogger<MenuCreatedEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Handle(MenuCreatedEvent notification, CancellationToken cancellationToken)
        {
            Validation(notification);
            var menu = Menu.Create(new MenuId(notification.StreamId!.Value), new Name(notification.Name));

            if (menu.IsFailed)
            {
                await StoreEventAsync(notification, false, cancellationToken);
                return;
            }

            var policyResult = await FallbackRetryPoicies.AsyncRetry.ExecuteAndCaptureAsync(() =>
                _restaurantRepository.AddMenuAsync(menu.Value, new RestaurantId(notification.RestaurantId), cancellationToken)
            );

            var isSuccess = policyResult.Outcome == OutcomeType.Successful;
            if (!isSuccess)
            {
                _logger.LogError("Consistency issue with {EventName} with {Id}", typeof(MenuCreatedEvent).Name, notification.StreamId);
            }
            await StoreEventAsync(notification, policyResult.Outcome == OutcomeType.Successful, cancellationToken);
        }

        private static void Validation(MenuCreatedEvent notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
        }

        private async Task StoreEventAsync(MenuCreatedEvent notification, bool success, CancellationToken cancellationToken)
        {
            var eventStorageResult = await FallbackRetryPoicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StorePendingEvent(notification, cancellationToken));

            if (eventStorageResult.Outcome == OutcomeType.Failure)
            {
                _logger.LogError("Event storage issue with {EventName} with {Id}. {Data}", typeof(MenuCreatedEvent).Name, notification.StreamId, notification.Serialize());
            }
        }
    }
}
