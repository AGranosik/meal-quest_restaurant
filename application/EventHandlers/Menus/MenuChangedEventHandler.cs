using application.EventHandlers.Interfaces;
using application.Restaurants.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using Microsoft.Extensions.Logging;
using Polly;
using RestaurantMenu = domain.Restaurants.Aggregates.Entities.Menu;
using Menu = domain.Menus.Aggregates.Menu;
using RestaurantMenuId = domain.Restaurants.ValueObjects.Identifiers.MenuId;
using RRestaurantId = domain.Restaurants.ValueObjects.Identifiers.RestaurantId;

namespace application.EventHandlers.Menus
{
    public class MenuChangedEventHandler(
            IEventInfoStorage<Menu, MenuId> eventInfoStorage,
            ILogger<AggregateChangedEventHandler<Menu, MenuId>> logger,
            IRestaurantRepository restaurantRepository,
            IEventEmitter<Restaurant> eventEmitter)
        : AggregateChangedEventHandler<Menu, MenuId>(eventInfoStorage, logger)
    {
        private readonly IRestaurantRepository _restaurantRepository = restaurantRepository;
        protected override async Task<bool> ProcessingEventAsync(AggregateChangedEvent<Menu, MenuId> notification, CancellationToken cancellationToken)
        {
            var policyResult = await FallbackRetryPolicies.AsyncRetry.ExecuteAndCaptureAsync(
                () => _restaurantRepository.AddMenuAsync(
                        RestaurantMenu.Create(
                            new RestaurantMenuId(notification.Aggregate.Id!.Value),
                            notification.Aggregate.Name!).Value,
                        new RRestaurantId(notification.Aggregate.Restaurant!.Value), cancellationToken));

            if (policyResult.Outcome == OutcomeType.Successful)
                return true;

            _logger.LogError(policyResult.FinalException, "Problem with handling menu repository save. {AggregateName}, Id: {Id}", nameof(Restaurant), notification.Aggregate.Id!.Value);

            return false;
        }
    }
}
