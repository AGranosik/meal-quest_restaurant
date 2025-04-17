using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;

namespace application.Menus.Commands.Interfaces;

public interface IMenuRepository
{
    Task<Result<List<MenuId>>> CreateMenuAsync(List<Menu> menus, CancellationToken cancellationToken);
    Task CreateRestaurantAsync(MenuRestaurant restaurant, CancellationToken cancellationToken);
}