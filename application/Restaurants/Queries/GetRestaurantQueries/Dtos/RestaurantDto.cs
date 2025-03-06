namespace application.Restaurants.Queries.GetRestaurantQueries.Dtos;

public record RestaurantDto(int Id, OwnerDto Owner, OpeningHorusDto OpeningHours);
public record OwnerDto(string Name, string Surname, AddressDto Address);
public record AddressDto(string Street, string City, double XCoordinate, double YCoordinate);
public record OpeningHorusDto(List<WorkingDayDto> WorkingDays);
public record WorkingDayDto(DayOfWeek Day, TimeOnly From, TimeOnly To);