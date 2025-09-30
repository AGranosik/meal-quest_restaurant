using application.Restaurants.Queries.Dto;
using application.Restaurants.Queries.OwnerRestaurantQueries.Dto;
using application.Restaurants.Queries.RestaurantDetailsQueries.Dto;

namespace application.Restaurants.Queries.Interfaces;

public interface IRestaurantQueryRepository
{
    Task<List<OwnerRestaurantDto>> GetRestaurantsForOwner(int ownerId, CancellationToken cancellationToken);
    Task<RestaurantDetailsDto?> GetRestaurantsForRestaurant(int restaurantId, CancellationToken cancellationToken);
}