using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;

namespace application.Menus.Commands.Interfaces
{
    public interface IMenuRepository
    {
        Task<Result<MenuId>> CreateMenuAsync(Menu menu, CancellationToken cancellationToken);
        Task AddRestaurantAsync(RestaurantIdMenuId restaurant, CancellationToken cancellationToken);
    }
}
