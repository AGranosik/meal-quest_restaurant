using application.Restaurants.Queries.Interfaces;
using application.Restaurants.Queries.RestaurantDetailsQueries.Dto;
using MediatR;

namespace application.Restaurants.Queries.RestaurantDetailsQueries;

public sealed record RestaurantDetailsQuery(int RestaurantId) : IRequest<RestaurantDetailsDto?>;

public sealed class RestaurantDetailsQueryHandler : IRequestHandler<RestaurantDetailsQuery, RestaurantDetailsDto?>
{
    private readonly IRestaurantQueryRepository _repository;

    public RestaurantDetailsQueryHandler(IRestaurantQueryRepository repository)
    {
        _repository = repository;
    }

    public Task<RestaurantDetailsDto?> Handle(RestaurantDetailsQuery request, CancellationToken cancellationToken)
        => _repository.GetRestaurantsForRestaurant(request.RestaurantId, cancellationToken);
}