using application.Restaurants.Queries.GetRestaurantQueries.Dtos;
using application.Restaurants.Queries.Interfaces;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.RestaurantContext.Repositories;

internal sealed class RestaurantQueryRepository : IRestaurantQueryRepository
{
    private readonly RestaurantDbContext _dbContext;

    public RestaurantQueryRepository(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<List<RestaurantDto>> GetRestaurantsForOwner(int ownerId, CancellationToken cancellationToken)
        => _dbContext.Restaurants
            .Where(r => r.Owner.Id! == new OwnerId(ownerId))
            .Select(r =>
                new RestaurantDto(
                    r.Id!.Value!,
                    new OwnerDto(r.Owner.Name.Value.Value, r.Owner.Surname.Value.Value, 
                        new AddressDto(r.Owner.Address.Street!.Value.Value, r.Owner.Address.City!.Value.Value, r.Owner.Address.Coordinates!.X, r.Owner.Address.Coordinates.Y)),
                    new OpeningHorusDto(r.OpeningHours.WorkingDays.Select(wd => new WorkingDayDto(wd.Day, wd.From, wd.To)).ToList()),
                    r.Description.Value.Value))
            .ToListAsync(cancellationToken);
}