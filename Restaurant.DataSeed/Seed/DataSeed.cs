using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using infrastructure.Database.RestaurantContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Controllers.Menus;
using webapi.Controllers.Menus.Requests;
using webapi.Controllers.Restaurants;
using webapi.Controllers.Restaurants.Requests;

namespace Restaurant.DataSeed.Seed;

public class DataSeed
{
    // use controller
    private readonly IMediator _mediator;
    private readonly ILogger<RestaurantController> _logger;
    private readonly RestaurantDbContext _restaurantContext;
    public DataSeed(IMediator mediator, ILogger<RestaurantController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }



    public async Task SeedRestaurants()
    {
        var restaurantController = new RestaurantController(_mediator, _logger);
        var menuController = new MenuController(_mediator);
        var restaurants = GenerateRestaurants(10000);
        var i = 0;
        foreach(var restaurant in restaurants)
        {
            i++;
            var result = await restaurantController.CreateRestaurantAsync(restaurant, CancellationToken.None);
            var okResult = result as OkObjectResult;
            var data = okResult.Value as RestaurantId;
            var restaurantId = data.Value;

            var menus = CreateMenus(10, restaurantId);
            foreach (var menu in menus)
            {
                await menuController.CreateMenuAsync(menu, CancellationToken.None);
            }

            Console.WriteLine($"Restaurant: {i}");
        }
    }

    //lodz cooridnates
    // Latitude: ~ 51.65°N to 51.85°N
    //Longitude: ~ 19.30°E to 19.65°E

    private static List<CreateRestaurantRequest> GenerateRestaurants(int n)
    {
        Random rand = new();
        //move it 
        var result = new List<CreateRestaurantRequest>(n);
        var openingHours = new List<short> { 6, 7, 8, 9 };
        var closingHours = new List<short> { 20, 21, 22, 23 };
        for(var i =0; i < n; i++)
        {
            var restaurantAddress = new CreateAddressRequest($"restaurant-seed-street-{i}", $"restaurant-city-seed-{i}", GetRandomDouble(rand, 16.30, 22.65), GetRandomDouble(rand, 50.65, 53.85));
            var owner = new CreateOwnerRequest($"owner-seed-name-{i}", $"owner-seed-surname-{i}", new CreateAddressRequest($"seed-street-{i}", $"city-seed-{i}", rand.NextDouble(), rand.NextDouble()));
            var openDays = Enumerable.Range(0, 7)
                .Select(d => new WorkingDayRequest((DayOfWeek)d, new DateTime(2020, 12, 12, openingHours[i%openingHours.Count], 00, 00), new DateTime(2020, 12, 12, closingHours[i%closingHours.Count], 00, 00))).ToList();
            var openinghours = new OpeningHoursRequest(openDays);
            result.Add(new CreateRestaurantRequest($"Data-seed-{i}", owner, openinghours, restaurantAddress));
        }

        return result;
    }

    private static List<CreateMenuRequest> CreateMenus(int n, int restaurantId)
    {
        Random rand = new();
        const int groupsPerMenu = 10;
        const int mealsPerGroup = 25;
        const string menuName = "menu-name-seed-";
        const string groupName = "group-name-seed-";
        const string mealName = "meal-name-seed-";
        const string categoryName = "cat-";
        
        var ingredients = Enumerable.Range(0, 1000)
            .Select(i => new CreateIngredientRequest($"ingredient-{i}")).ToList();
        
        var categories = Enumerable.Range(0, 1000)
            .Select(i => $"{categoryName}{i}").ToList();
        var menus  = new List<CreateMenuRequest>(n);
        var groups = new List<CreateGroupRequest>(groupsPerMenu);
        var meals = new List<CreateMealRequest>(mealsPerGroup);
        
        for (int i = 0; i < n; i++)
        {
            var name = restaurantId + menuName + i;
            groups.Clear();
            for (var j = 0; j < groupsPerMenu; j++)
            {
                meals.Clear();
                for (var k = 0; k < mealsPerGroup; k++)
                {
                    var meal = new CreateMealRequest(
                        name + mealName + i + j + k, 
                        2,
                        GetRandomNumbersOfElements(ingredients, rand, 15),
                        GetRandomNumbersOfElements(categories, rand, 12));
                    
                    meals.Add(meal);
                }
                var group = new CreateGroupRequest(restaurantId + groupName + i + j, [..meals]);
                groups.Add(group);
            }
            
            menus.Add(new CreateMenuRequest(name, [..groups], restaurantId));
        }
        
        return  menus;
    }

    private static double GetRandomDouble(Random random, double min, double max)
    {
        return min + (random.NextDouble() * (max - min));
    }

    private static List<T> GetRandomNumbersOfElements<T>(List<T> source, Random random, int numberOfElements)
        => [..source.OrderBy(x => random.Next()).Take(Math.Min(numberOfElements, source.Count)).ToList()];
}