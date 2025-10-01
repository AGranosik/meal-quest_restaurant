using application.Menus.Queries.Dto;

namespace application.Menus.Queries.Interfaces;

public interface IReadMenuRepository
{
    Task<MenuRestaurantDto?> GetRestaurantMenu(int restaurantId, CancellationToken cancellationToken);
}