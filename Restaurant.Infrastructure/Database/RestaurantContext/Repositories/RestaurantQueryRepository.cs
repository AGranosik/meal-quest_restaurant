using application.Restaurants.Queries.GetRestaurantQueries.Dtos;
using application.Restaurants.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.RestaurantContext.Repositories
{
    public sealed class RestaurantQueryRepository(RestaurantDbContext dbContext) : IRestaurantQueryRepository
    {
        private readonly RestaurantDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public Task<List<RestaurantDto>> GetRestaurantsForOwner(int ownerId, CancellationToken cancellationToken)
            => _dbContext.Restaurants
            .Where(r => r.Owner.Id == new domain.Restaurants.ValueObjects.Identifiers.OwnerId(ownerId))
            .Select(r => new RestaurantDto(null, null))
            .ToListAsync(cancellationToken);
    }
}
