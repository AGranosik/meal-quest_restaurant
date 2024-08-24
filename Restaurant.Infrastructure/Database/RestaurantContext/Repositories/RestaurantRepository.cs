using application.Restaurants.Commands.Interfaces;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace infrastructure.Database.RestaurantContext.Repositories
{
    public class RestaurantReposiotry : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantReposiotry(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddMenuAsync(Menu menu, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<RestaurantId?>> CreateAsync(Restaurant restaurant, CancellationToken cancellationToken)
        {
            //var dbModel = Restaurant.CastToDbModel(restaurant);
            _dbContext.Restaurants.Add(restaurant);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(restaurant.Id);
        }
    }
}
