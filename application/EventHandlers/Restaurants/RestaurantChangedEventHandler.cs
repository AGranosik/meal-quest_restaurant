using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.Extensions.Logging;
using Polly;

namespace application.EventHandlers.Restaurants
{
    internal sealed class RestaurantChangedEventHandler
        (IEventInfoStorage<Restaurant, RestaurantId> eventInfoStorage,
        ILogger<AggregateChangedEventHandler<Restaurant, RestaurantId>> logger,
        IEventEmitter<Restaurant> eventEmitter,
        IMenuRepository menuRepository)
        : AggregateChangedEventHandler<Restaurant, RestaurantId>(eventInfoStorage, logger, eventEmitter)
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        protected override async Task<bool> ProcessEventAsync(AggregateChangedEvent<Restaurant, RestaurantId> notification, CancellationToken cancellationToken)
        {
            var policyResult = await FallbackRetryPolicies.AsyncRetry.ExecuteAndCaptureAsync(
                () => _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.Aggregate.Id!.Value), cancellationToken));

            if (policyResult.Outcome == OutcomeType.Successful)
                return true;

            _logger.LogError(policyResult.FinalException, "Problem with handling menu repository save. {AggregateName}, Id: {Id}", nameof(Restaurant), notification.Aggregate.Id!.Value);

            return false;
        }
    }
}
