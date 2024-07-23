using application.Restaurants.Commands;

namespace webapi.Controllers.Restaurants.Requests
{
    public record CreateRestaurantRequest(CreateOwnerRequest? Owner, OpeningHoursRequest? OpeningHours)
    {
        public CreateRestaurantCommand CastoCommand()
        {
            var address = new CreateAddressCommand(Owner?.Address?.Street, Owner?.Address?.City, Owner?.Address?.XCoordinate ?? 0, Owner?.Address?.YCoordinate ?? 0);
            var owner = new CreateOwnerCommand(Owner?.Name, Owner?.Surname, address);
            var openingHours = new OpeningHoursCommand(OpeningHours?.WorkingDays.Select(wd => new WorkingDayCommand(DayOfWeek.Monday, wd.From, wd.To)).ToList() ?? []);
            return new CreateRestaurantCommand(owner, openingHours);
        }
    }

    public record CreateOwnerRequest(string? Name, string? Surname, CreateAddressRequest? Address);

    public record CreateAddressRequest(string? Street, string? City, double XCoordinate, double YCoordinate);

    public record OpeningHoursRequest(List<WorkingDayRequest> WorkingDays);

    public record WorkingDayRequest(DayOfWeek Day, DateTime From, DateTime To);
}
