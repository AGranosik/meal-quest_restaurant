﻿using domain.Common.BaseTypes;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates
{
    public class Restaurant: Aggregate<RestaurantId>
    {
        private List<MenuRestaurantId> _menus = new(); // if there's gonna be need more info about menu then Menu entity in Resuraurant context will be added.

        public Owner Owner { get; }
        public OpeningHours OpeningHours { get; }
        public static Result<Restaurant> Create(RestaurantId id, Owner owner, OpeningHours openingHours)
        {
            var creationResult = CreationValidation(id, owner, openingHours);
            if (creationResult.IsFailed)
                return creationResult;

            return Result.Ok(new Restaurant(id, owner, openingHours));
        }

        public Result AddMenu(MenuRestaurantId menuId) // taking advantage of same project
        {
            if (_menus.Contains(menuId))
                return Result.Fail("Menu already at restaurant.");

            _menus.Add(menuId);
            return Result.Ok();
        }

        private Restaurant(RestaurantId id, Owner owner, OpeningHours openingHours) : base(id)
        {
            Owner = owner;
            OpeningHours = openingHours;
        }

        private static Result CreationValidation(RestaurantId id, Owner owner, OpeningHours openingHours)
        {
            if (owner is null)
                return Result.Fail("Owner cannot be null.");

            if (openingHours is null)
                return Result.Fail("Owner cannot be null.");

            return Result.Ok();
        }


    }
}
