using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace application.Restaurants.Commands.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Result<RestaurantId?>> CreateAsync(Restaurant restaurant, CancellationToken cancellationToken);
    }
}
