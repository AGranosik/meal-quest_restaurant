using application.Restaurants.Queries.GetRestaurantQueries.Dtos;

namespace application.Restaurants.Queries.Interfaces
{
    public interface IRestaurantQueryRepository
    {
        Task<List<RestaurantDto>> GetRestaurantsForOwner(int ownerId, CancellationToken cancellationToken);
    }
}
