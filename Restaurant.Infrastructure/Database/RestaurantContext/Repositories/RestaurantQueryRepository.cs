using application.Restaurants.Queries.Dto;
using application.Restaurants.Queries.Interfaces;
using application.Restaurants.Queries.OwnerRestaurantQueries.Dto;
using application.Restaurants.Queries.RestaurantDetailsQueries.Dto;
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

    public Task<List<OwnerRestaurantDto>> GetRestaurantsForOwner(int ownerId, CancellationToken cancellationToken)
        => _dbContext.Restaurants
            .AsNoTracking()
            .Where(r => r.Owner.Id! == new OwnerId(ownerId))
            .Select(r =>
                new OwnerRestaurantDto(
                    r.Id!.Value!,
                    new OwnerDto(r.Owner.Name.Value.Value, r.Owner.Surname.Value.Value, 
                        new AddressDto(r.Owner.Address.Street!.Value.Value, r.Owner.Address.City!.Value.Value, r.Owner.Address.Coordinates!.X, r.Owner.Address.Coordinates.Y)),
                    new OpeningHoursDto(r.OpeningHours.WorkingDays.Select(wd => new WorkingDayDto(wd.Day, wd.From, wd.To)).ToList()),
                    r.Description.Value.Value,
                    r.Logo == null ? null : Convert.ToBase64String(r.Logo.Data!)))
            .ToListAsync(cancellationToken);

    public Task<RestaurantDetailsDto?> GetRestaurantsForRestaurant(int restaurantId, CancellationToken cancellationToken)
        => _dbContext.Restaurants
            .AsNoTracking()
            .Where(r => r.Id! == new RestaurantId(restaurantId))
            .Select(r => new RestaurantDetailsDto(
                 r.Id!.Value!, new OpeningHoursDto(r.OpeningHours.WorkingDays.Select(wd => new WorkingDayDto(wd.Day, wd.From, wd.To)).ToList()),
                 new AddressDto(r.Address.Street!.Value.Value, r.Address.City.Value.Value, r.Address.Coordinates!.X, r.Address.Coordinates.Y),
                 r.Description.Value.Value,
                 r.Logo == null ? null : Convert.ToBase64String(r.Logo.Data!)
                 ))
            .FirstOrDefaultAsync(cancellationToken);
}