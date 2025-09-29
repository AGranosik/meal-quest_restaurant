using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using infrastructure.Database.RestaurantContext;
using Microsoft.AspNetCore.Http;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Restaurants.DataMocks;

internal static class RestaurantDataFaker
{
    internal static CreateRestaurantRequest ValidRequest()
    {
        var openingHours = new OpeningHoursRequest(
        [
            new(DayOfWeek.Monday, new DateTime(12, 12, 12, 11, 00, 00), new DateTime(12, 12, 12, 13, 00, 00)),
            new(DayOfWeek.Tuesday, new DateTime(12, 12, 12, 11, 00, 00), new DateTime(12, 12, 12, 13, 00, 00)),
            new(DayOfWeek.Wednesday, new DateTime(12, 12, 12, 11, 00, 00), new DateTime(12, 12, 12, 13, 00, 00)),
            new(DayOfWeek.Thursday, new DateTime(12, 12, 12, 11, 00, 00) ,new DateTime(12, 12, 12, 13, 00, 00)),
            new(DayOfWeek.Friday, new DateTime(12, 12, 12, 11, 00, 00), new DateTime(12, 12, 12, 13, 00, 00)),
            new(DayOfWeek.Saturday, new DateTime(12, 12, 12, 11, 00, 00), new DateTime(12, 12, 12, 13, 00, 00)),
            new(DayOfWeek.Sunday, new DateTime(12, 12, 12, 11, 00, 00), new DateTime(12, 12, 12, 13, 00, 00)),
        ]);
        var address = new CreateAddressRequest("street", "City", 0, 0);
        var owner = new CreateOwnerRequest("name", "surname", address);
        var currentDir = Directory.GetCurrentDirectory();
        var logoPath = Path.Combine(currentDir, "..\\..\\..\\Restaurants\\logos\\1.jpeg");
        var fileStream = new FileStream(logoPath, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "1.jpeg", Path.GetFileName(logoPath))
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
        return new CreateRestaurantRequest("restaurantName", owner, openingHours, address, "Description.", formFile);
    }
         
    
    public static async Task<List<Restaurant>> AddRestaurants(int numberOfRestaurants, int restaurantsPerOwner, RestaurantDbContext dbContext, CancellationToken cancellationToken)
    {
        var restaurants = new List<Restaurant>(numberOfRestaurants);

        var iRestaurant = 0;
        var restaurantName = "test";
        while (iRestaurant < numberOfRestaurants)
        {
            var iOwner = 0;
            var restaurantAddress = Address.Create(new Street("street"), new City("city"), new Coordinates(10, 10)).Value;
            var owner = Owner.Create(new Name("test" + iRestaurant), new Name("surname"), Address.Create(new Street("street"), new City("city"), new Coordinates(10, 10)).Value).Value;

            while(iOwner < restaurantsPerOwner)
            {
                var openingHours = OpeningHours.Create(
                [
                    WorkingDay.Create(DayOfWeek.Monday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                    WorkingDay.FreeDay(DayOfWeek.Tuesday).Value,
                    WorkingDay.Create(DayOfWeek.Wednesday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                    WorkingDay.Create(DayOfWeek.Thursday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                    WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                    WorkingDay.Create(DayOfWeek.Saturday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                    WorkingDay.Create(DayOfWeek.Sunday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                ]).Value;

                var currentDir = Directory.GetCurrentDirectory();
                var logoPath = Path.Combine(currentDir, "..\\..\\..\\Restaurants\\logos\\1.jpeg");
                var logo = File.ReadAllBytes(logoPath);
                var restaurant = Restaurant.Create(new Name(restaurantName + iRestaurant), owner, openingHours , restaurantAddress, new Description("description"), new RestaurantLogo(logo));
                restaurants.Add(restaurant.Value);
                iOwner++;
                iRestaurant++;
            }
        }
        dbContext.Restaurants.AddRange(restaurants);
        await dbContext.SaveChangesAsync(cancellationToken);
        return restaurants;
    }
}