using application.EventHandlers.Interfaces;
using application.Restaurants.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using Microsoft.Extensions.Logging;
using Polly;

namespace application.EventHandlers.Menus
{
    public class MenuChangedEventHandler(IEventInfoStorage<domain.Menus.Aggregates.Menu, MenuId> eventInfoStorage, ILogger<AggregateChangedEventHandler<domain.Menus.Aggregates.Menu, MenuId>> logger, IRestaurantRepository restaurantRepository) : AggregateChangedEventHandler<domain.Menus.Aggregates.Menu, MenuId>(eventInfoStorage, logger)
    {
        private readonly IRestaurantRepository _restaurantRepository = restaurantRepository;
        protected override async Task<bool> ProcessingEventAsync(AggregateChangedEvent<domain.Menus.Aggregates.Menu, MenuId> notification, CancellationToken cancellationToken)
        {
            // TODO: clean up mess with namespaces
            var policyResult = await FallbackRetryPolicies.AsyncRetry.ExecuteAndCaptureAsync(
                () => _restaurantRepository.AddMenuAsync(
                        domain.Restaurants.Aggregates.Entities.Menu.Create(
                            new domain.Restaurants.ValueObjects.Identifiers.MenuId(notification.Aggregate.Id!.Value),
                            notification.Aggregate.Name).Value,
                        new domain.Restaurants.ValueObjects.Identifiers.RestaurantId(notification.Aggregate.Restaurant.Value), cancellationToken));

            if (policyResult.Outcome == OutcomeType.Successful)
                return true;

            _logger.LogError(policyResult.FinalException, "Problem with handling menu repository save. {AggregateName}, Id: {Id}", nameof(Restaurant), notification.Aggregate.Id!.Value);

            return false;
        }
    }
}
