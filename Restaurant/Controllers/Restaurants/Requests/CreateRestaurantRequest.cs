using application.Restaurants.Commands;

namespace webapi.Controllers.Restaurants.Requests;

public record CreateRestaurantRequest
{
    public CreateRestaurantRequest(string? Name, CreateOwnerRequest? Owner, OpeningHoursRequest? OpeningHours, CreateAddressRequest? Address, string Description)
    {
        this.Name = Name;
        this.Owner = Owner;
        this.OpeningHours = OpeningHours;
        this.Address = Address;
        this.Description = Description;
    }

    public CreateRestaurantCommand CastoCommand()
    {
        var ownerAddress = new CreateAddressCommand(Owner?.Address?.Street, Owner?.Address?.City, Owner?.Address?.XCoordinate ?? 0, Owner?.Address?.YCoordinate ?? 0);
        var owner = new CreateOwnerCommand(Owner?.Name, Owner?.Surname, ownerAddress);
        var openingHours = new OpeningHoursCommand(OpeningHours?.WorkingDays.Select(wd => new WorkingDayCommand(wd.Day, wd.From, wd.To)).ToList() ?? []);
        var restaurantAddress = new CreateAddressCommand(Address?.Street, Address? .City, Address?.XCoordinate ?? 0, Address?.YCoordinate ?? 0);
        return new CreateRestaurantCommand(Name, owner, openingHours, restaurantAddress, Description);
    }

    public string? Name { get; init; }
    public CreateOwnerRequest? Owner { get; init; }
    public OpeningHoursRequest? OpeningHours { get; init; }
    public CreateAddressRequest? Address { get; init; }
    public string Description { get; init; }
}

public record CreateOwnerRequest
{
    public CreateOwnerRequest(string? Name, string? Surname, CreateAddressRequest? Address)
    {
        this.Name = Name;
        this.Surname = Surname;
        this.Address = Address;
    }

    public string? Name { get; init; }
    public string? Surname { get; init; }
    public CreateAddressRequest? Address { get; init; }
}

public record CreateAddressRequest
{
    public CreateAddressRequest(string? Street, string? City, double XCoordinate, double YCoordinate)
    {
        this.Street = Street;
        this.City = City;
        this.XCoordinate = XCoordinate;
        this.YCoordinate = YCoordinate;
    }

    public string? Street { get; init; }
    public string? City { get; init; }
    public double XCoordinate { get; init; }
    public double YCoordinate { get; init; }
}

public record OpeningHoursRequest
{
    public OpeningHoursRequest(List<WorkingDayRequest> WorkingDays)
    {
        this.WorkingDays = WorkingDays;
    }

    public List<WorkingDayRequest> WorkingDays { get; init; }
}

public record WorkingDayRequest
{
    public WorkingDayRequest(DayOfWeek Day, DateTime From, DateTime To)
    {
        this.Day = Day;
        this.From = From;
        this.To = To;
    }

    public DayOfWeek Day { get; init; }
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}