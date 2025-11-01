using application.EventHandlers.Interfaces;
using application.Menus.Commands.Interfaces;
using core.Operations.FallbackPolicies;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.Extensions.Logging;
using Polly;

namespace application.EventHandlers.Restaurants;

internal sealed class RestaurantChangedEventHandler : AggregateChangedEventHandler<Restaurant, RestaurantId>
{
    private readonly IMenuRepository _menuRepository;

    public RestaurantChangedEventHandler(IEventInfoStorage<Restaurant, RestaurantId> eventInfoStorage,
        ILogger<AggregateChangedEventHandler<Restaurant, RestaurantId>> logger,
        IEventEmitter<Restaurant> eventEmitter,
        IMenuRepository menuRepository) : base(eventInfoStorage, logger, eventEmitter)
    {
        _menuRepository = menuRepository;
    }

    protected override async Task<bool> ProcessEventAsync(AggregateChangedEvent<Restaurant, RestaurantId> notification, CancellationToken cancellationToken)
    {
        var policyResult = await FallbackRetryPolicies.AsyncRetry.ExecuteAndCaptureAsync(
            () => _menuRepository.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(notification.Aggregate.Id!.Value)), cancellationToken));

        if (policyResult.Outcome == OutcomeType.Successful)
            return true;

        Logger.LogError(policyResult.FinalException, "Problem with handling menu repository save. {AggregateName}, Id: {Id}", nameof(Restaurant), notification.Aggregate.Id!.Value);

        return false;
    }
}