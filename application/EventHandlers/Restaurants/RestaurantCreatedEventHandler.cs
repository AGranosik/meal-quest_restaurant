using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using MediatR;
using Polly;

namespace application.EventHandlers.Restaurants
{
    public class RestaurantCreatedEventHandler(IMenuRepository menuRepository, IEventInfoStorage<RestaurantCreatedEvent> eventInfoStorage) : INotificationHandler<RestaurantCreatedEvent>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        private readonly IEventInfoStorage<RestaurantCreatedEvent> _eventInfoStorage = eventInfoStorage ?? throw new ArgumentNullException();

        public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            Validation(notification);

            var policyResult = await FallbackRetryPoicies.AsyncRetry.ExecuteAndCaptureAsync(() =>
                _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.Id.Value), cancellationToken));

            if(policyResult.Outcome == OutcomeType.Successful)
                await StoreEventAsync(notification, cancellationToken);
            else
            {
                //log
            }
        }

        private static void Validation(RestaurantCreatedEvent notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notification.Id);
        }

        private async Task StoreEventAsync(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventStorageResult = await FallbackRetryPoicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StoreEventAsync(notification, cancellationToken));

            if (eventStorageResult.Outcome == OutcomeType.Failure)
            {
                // log
            }//logs in the future or throw exception
        }
    }
}
