﻿using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;

namespace application.Menus.Commands.Interfaces;

public interface IMenuRepository
{
    Task<Result<MenuId>> CreateMenuAsync(Menu menu, CancellationToken cancellationToken);
    Task CreateRestaurantAsync(MenuRestaurant restaurant, CancellationToken cancellationToken);
}