using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using MediatR;
using Polly;

namespace application.EventHandlers.Restaurants
{
    // meake handlers sealed
    public class RestaurantCreatedEventHandler(IMenuRepository menuRepository, IEventInfoStorage<RestaurantEvent> eventInfoStorage) : INotificationHandler<RestaurantCreatedEvent>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        private readonly IEventInfoStorage<RestaurantEvent> _eventInfoStorage = eventInfoStorage ?? throw new ArgumentNullException();

        public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            Validation(notification);

            var policyResult = await FallbackRetryPoicies.AsyncRetry.ExecuteAndCaptureAsync(() =>
                _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.StreamId!.Value), cancellationToken));

            await StoreEventAsync(notification, policyResult.Outcome == OutcomeType.Successful, cancellationToken);
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
                //throw specific exception (eventual incosistency exception??)
            }//logs in the future or throw exception
        }
    }
}
