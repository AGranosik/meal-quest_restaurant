using application.Restaurants.Queries.Dto;

namespace application.Restaurants.Queries.RestaurantDetailsQueries.Dto;

public sealed record RestaurantDetailsDto(int Id,
    OpeningHoursDto OpeningHours,
    AddressDto Address,
    string Description,
    string Base64Logo);