using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using infrastructure.Database.RestaurantContext.Models;

namespace infrastructure.Database.RestaurantContext.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Result<RestaurantId?>> CreateAsync(domain.Restaurants.Aggregates.Restaurant restaurant, CancellationToken cancellationToken);
    }

    public class RestaurantReposiotry : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantReposiotry(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<RestaurantId?>> CreateAsync(domain.Restaurants.Aggregates.Restaurant restaurant, CancellationToken cancellationToken)
        {
            //var dbModel = Restaurant.CastToDbModel(restaurant);
            _dbContext.Restaurants.Add(restaurant);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(restaurant.Id);
        }
    }
}
