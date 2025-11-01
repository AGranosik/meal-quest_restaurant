using application.Restaurants.Queries.Dto;

namespace application.Restaurants.Queries.OwnerRestaurantQueries.Dto;

public sealed record OwnerRestaurantDto(
    int Id,
    OwnerDto Owner,
    OpeningHoursDto OpeningHours,
    string Description,
    string? Base64Logo);





