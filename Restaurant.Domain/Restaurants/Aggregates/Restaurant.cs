﻿using domain.Common.BaseTypes;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates
{
    public class Restaurant: Aggregate<RestaurantId>
    {
        public static Result<Restaurant> Create(RestaurantId id, Owner owner, OpeningHours openingHours, Coordinates coordinates)
        {
            CreationValidation(id, owner, openingHours);
            return Result.Ok(new Restaurant(id, owner, openingHours, coordinates));
        }

        private Restaurant(RestaurantId id, Owner owner, OpeningHours openingHours, Coordinates coordinates) : base(id)
        {
            Owner = owner;
            OpeningHours = openingHours;
            Coordinates = coordinates;
        }

        private static Result CreationValidation(RestaurantId id, Owner owner, OpeningHours openingHours)
        {
            if (owner is null)
                return Result.Fail("Owner cannot be null.");

            if (openingHours is null)
                return Result.Fail("Owner cannot be null.");

            return Result.Ok();
        }

        public Owner Owner { get; }
        public OpeningHours OpeningHours { get; }
        public Coordinates Coordinates { get; }
    }
}
