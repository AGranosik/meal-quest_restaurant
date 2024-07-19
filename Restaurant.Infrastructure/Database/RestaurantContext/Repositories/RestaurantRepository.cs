using domain.Restaurants.Aggregates;
using FluentResults;

namespace infrastructure.Database.RestaurantContext.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Result> CreateAsync(Restaurant restaurant);
    }

    public class RestaurantReposiotry : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantReposiotry(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Result> CreateAsync(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
