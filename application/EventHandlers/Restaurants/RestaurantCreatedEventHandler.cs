using System.Runtime.CompilerServices;
using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using MediatR;
using Polly;
using Polly.Fallback;

namespace application.EventHandlers.Restaurants
{
    public class RestaurantCreatedEventHandler(IMenuRepository menuRepository, IEventInfoStorage<RestaurantCreatedEvent> eventInfoStorage) : INotificationHandler<RestaurantCreatedEvent>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        private readonly IEventInfoStorage<RestaurantCreatedEvent> _eventInfoStorage = eventInfoStorage ?? throw new ArgumentNullException();

        public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventStorageResult = await FallbackRetryPoicies.AsyncRetry
                .ExecuteAndCaptureAsync(() => _eventInfoStorage.StoreEventAsync(notification, cancellationToken));

            if(eventStorageResult.Outcome == OutcomeType.Failure)//logs in the future or throw exception
                return;

            var eventInfoId = eventStorageResult.Result;

            AsyncFallbackPolicy fallbackPolicy = Policy.Handle<Exception>()
                .FallbackAsync(async CallConvSuppressGCTransition =>
                {
                    await _eventInfoStorage.MarkAsNotSentAsync(eventInfoId, cancellationToken);
                });

            var wrappedPolicy = Policy.WrapAsync(FallbackRetryPoicies.AsyncRetry, fallbackPolicy);
            var policyResult = await wrappedPolicy.ExecuteAndCaptureAsync(() =>
                _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.Id.Value), cancellationToken));

            if(policyResult.Outcome == OutcomeType.Successful)
                await _eventInfoStorage.MarkAsSentAsync(eventInfoId, cancellationToken);
        }

        private void Validation(RestaurantCreatedEvent notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(notification.Id);
        }
    }
}
