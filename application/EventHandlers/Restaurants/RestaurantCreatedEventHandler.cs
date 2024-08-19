using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using MediatR;

namespace application.EventHandlers.Restaurants
{
    public class RestaurantCreatedEventHandler(IMenuRepository menuRepository) : INotificationHandler<RestaurantCreatedEvent>
    {
        private readonly IMenuRepository _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));

        //store events?? -> mongo?
        // fallback policy
        // save event before sent 
        // udpate status after sent
        public async Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
        {
            // check if there is goona be more than 1 action that polly retires the first as well
            await FallbackRetryPoicies.AsyncRetry
                .ExecuteAsync(() => _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.Restaurant.Id!.Value), cancellationToken));
        }
    }
}
