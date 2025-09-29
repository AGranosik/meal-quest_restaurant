using application.Restaurants.Queries.Dto;
using application.Restaurants.Queries.Interfaces;
using application.Restaurants.Queries.OwnerRestaurantQueries.Dto;
using MediatR;

namespace application.Restaurants.Queries.OwnerRestaurantQueries;

public sealed class OwnerRestaurantQuery : IRequest<List<OwnerRestaurantDto>>
{
    public required int OwnerId { get; set; }
}

public sealed class OwnerRestaurantQueryHandler : IRequestHandler<OwnerRestaurantQuery, List<OwnerRestaurantDto>>
{
    private readonly IRestaurantQueryRepository _repository;

    public OwnerRestaurantQueryHandler(IRestaurantQueryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<List<OwnerRestaurantDto>> Handle(OwnerRestaurantQuery request, CancellationToken cancellationToken)
        => _repository.GetRestaurantsForOwner(request.OwnerId, cancellationToken);
}