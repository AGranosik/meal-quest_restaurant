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
            
}