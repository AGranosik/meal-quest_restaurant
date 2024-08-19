using application.Menus.Commands.Interfaces;
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
        // microsoft resilence
        // save event before sent 
        // udpate status after sent
        public Task Handle(RestaurantCreatedEvent notification, CancellationToken cancellationToken)
            => _menuRepository.AddRestaurantAsync(new RestaurantIdMenuId(notification.Restaurant.Id!.Value), cancellationToken);
    }
}
