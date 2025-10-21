using application.Menus.Queries.Dto;
using application.Menus.Queries.Interfaces;
using MediatR;

namespace application.Menus.Queries;

public sealed record GetRestaurantMenusQuery(int RestaurantId) : IRequest<MenuRestaurantDto?>;

public sealed class GetRestaurantMenusQueryHandler : IRequestHandler<GetRestaurantMenusQuery, MenuRestaurantDto?>
{
    private readonly IReadMenuRepository _repository;

    public GetRestaurantMenusQueryHandler(IReadMenuRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    //TODO: filt based on isActive flag
    public Task<MenuRestaurantDto?> Handle(GetRestaurantMenusQuery request, CancellationToken cancellationToken)
        => _repository.GetRestaurantMenu(request.RestaurantId, cancellationToken);
}