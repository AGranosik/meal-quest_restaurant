using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Common.BaseTypes;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.Extensions.Logging;

namespace application.EventHandlers.Restaurants
{
    public class RestaurantChangedEventHandler(IEventInfoStorage<Restaurant, RestaurantId> eventInfoStorage, ILogger<AggregateChangedEventHandler<Restaurant, RestaurantId>> logger, IMenuRepository menuRepository) : AggregateChangedEventHandler<Restaurant, RestaurantId>(eventInfoStorage, logger)
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        // return status.
        // it what if ther's gonna be more options and only one of them fail?
        // each method should not throw erro on duplicates?
        protected override async Task ProcessingEventAsync(AggregateChangedEvent<Restaurant, RestaurantId> notification, CancellationToken cancellationToken)
        {
            var policyResult = await FallbackRetryPoicies.AsyncRetry.ExecuteAndCaptureAsync(
                () => _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.Aggregate.Id!.Value), cancellationToken));
        }
    }
}
