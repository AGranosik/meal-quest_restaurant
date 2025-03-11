namespace application.Restaurants.Queries.GetRestaurantQueries.Dtos;

public record RestaurantDto
{
    public RestaurantDto(int Id, OwnerDto Owner, OpeningHorusDto OpeningHours)
    {
        this.Id = Id;
        this.Owner = Owner;
        this.OpeningHours = OpeningHours;
    }

    public int Id { get; init; }
    public OwnerDto Owner { get; init; }
    public OpeningHorusDto OpeningHours { get; init; }
}

public record OwnerDto
{
    public OwnerDto(string Name, string Surname, AddressDto Address)
    {
        this.Name = Name;
        this.Surname = Surname;
        this.Address = Address;
    }

    public string Name { get; init; }
    public string Surname { get; init; }
    public AddressDto Address { get; init; }
}

public record AddressDto
{
    public AddressDto(string Street, string City, double XCoordinate, double YCoordinate)
    {
        this.Street = Street;
        this.City = City;
        this.XCoordinate = XCoordinate;
        this.YCoordinate = YCoordinate;
    }

    public string Street { get; init; }
    public string City { get; init; }
    public double XCoordinate { get; init; }
    public double YCoordinate { get; init; }
}

public record OpeningHorusDto
{
    public OpeningHorusDto(List<WorkingDayDto> WorkingDays)
    {
        this.WorkingDays = WorkingDays;
    }

    public List<WorkingDayDto> WorkingDays { get; init; }
}

public record WorkingDayDto
{
    public WorkingDayDto(DayOfWeek Day, TimeOnly From, TimeOnly To)
    {
        this.Day = Day;
        this.From = From;
        this.To = To;
    }

    public DayOfWeek Day { get; init; }
    public TimeOnly From { get; init; }
    public TimeOnly To { get; init; }
}