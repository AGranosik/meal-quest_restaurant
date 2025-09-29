namespace application.Restaurants.Queries.Dto;

public sealed record OpeningHoursDto(List<WorkingDayDto> WorkingDays);
public sealed record WorkingDayDto(DayOfWeek Day, TimeOnly From, TimeOnly To);