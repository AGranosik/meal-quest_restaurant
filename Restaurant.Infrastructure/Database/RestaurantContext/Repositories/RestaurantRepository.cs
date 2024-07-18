using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using infrastructure.Database.Common;

namespace infrastructure.Database.RestaurantContext.Repositories
{
    public interface IRestaurantRepository : IAggregateRepository<Restaurant, RestaurantId> { }

    public class RestaurantReposiotry : IRestaurantRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantReposiotry(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> SaveAsync(Restaurant aggregate, CancellationToken cancellationToken)
        {

        }

        // cannot use events because its after save not before :/
        // some other name convention?
        private async Task RestaurantCreationAsync<RestaurantCreated>(RestaurantCreated @)
    }
}
