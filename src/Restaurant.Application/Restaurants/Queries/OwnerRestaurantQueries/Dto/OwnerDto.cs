using application.Restaurants.Queries.Dto;

namespace application.Restaurants.Queries.OwnerRestaurantQueries.Dto;

public record OwnerDto(string Name, string Surname, AddressDto Address);