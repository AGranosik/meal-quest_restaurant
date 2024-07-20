using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using infrastructure.Database.RestaurantContext.Models;

namespace infrastructure.Database.RestaurantContext.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Result<RestaurantId?>> CreateAsync(domain.Restaurants.Aggregates.Restaurant restaurant, CancellationToken cancellationToken);
    }

    internal class RestaurantReposiotry : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;

        internal RestaurantReposiotry(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<RestaurantId?>> CreateAsync(domain.Restaurants.Aggregates.Restaurant restaurant, CancellationToken cancellationToken)
        {
            var dbModel = Restaurant.CastToDbModel(restaurant);
            _dbContext.Restaurants.Add(dbModel);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(dbModel.Id);
        }
    }
}
