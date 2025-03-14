﻿using application.Restaurants.Commands.Interfaces;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.RestaurantContext.Repositories;

internal class RestaurantReposiotry : IRestaurantRepository
{
    private readonly RestaurantDbContext _dbContext;

    public RestaurantReposiotry(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddMenuAsync(Menu menu, RestaurantId restaurantId, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id! == restaurantId, cancellationToken);
        if (restaurant is null)
            throw new ArgumentException("Restaurant doesnt exist.");

        var result = restaurant.AddMenu(menu);

        // if not saved that means some serious issue which has to be chacked
        if(result.IsFailed)
            throw new ArgumentException(result.Errors.ToString());

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<RestaurantId?>> CreateAsync(Restaurant restaurant, CancellationToken cancellationToken)
    {
        _dbContext.Restaurants.Add(restaurant);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok(restaurant.Id);
    }
}