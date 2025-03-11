using application.Restaurants.Queries.GetRestaurantQueries.Dtos;
using application.Restaurants.Queries.Interfaces;
using MediatR;

namespace application.Restaurants.Queries.GetRestaurantQueries;

public sealed class GetRestaurantQuery : IRequest<List<RestaurantDto>>
{
    public required int OwnerId { get; set; }
}

public sealed class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, List<RestaurantDto>>
{
    private readonly IRestaurantQueryRepository _repository;

    public GetRestaurantQueryHandler(IRestaurantQueryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<List<RestaurantDto>> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
        => _repository.GetRestaurantsForOwner(request.OwnerId, cancellationToken);
}