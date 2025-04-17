using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using infrastructure.Database.RestaurantContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using webapi.Controllers.Menus;
using webapi.Controllers.Menus.Requests;
using webapi.Controllers.Restaurants;
using webapi.Controllers.Restaurants.Requests;

namespace Restaurant.DataSeed.Seed;

public class DataSeed
{
    // use controller
    private readonly IServiceProvider _serviceProvider;

    // private readonly IMediator _mediator;
    private readonly ILogger<RestaurantController> _logger;

    public DataSeed(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = NullLogger<RestaurantController>.Instance;
    }

    public async Task SeedRestaurants()
    {
        var openingHours = new List<short> { 6, 7, 8, 9 };
        var closingHours = new List<short> { 20, 21, 22, 23 };
        const string categoryName = "cat-";
        var restaurants = GenerateRestaurants(10, openingHours, closingHours);
        var ingredients = Enumerable.Range(0, 1000)
            .Select(i => new CreateIngredientRequest($"ingredient-{i}")).ToList();

        var categories = Enumerable.Range(0, 1000)
            .Select(i => $"{categoryName}{i}").ToList();
        var restaurantsId = new List<int>(restaurants.Count);
        
        var i = 0;
        const int tasksLimit = 10;
        var tasks = new List<Task>(tasksLimit);
        foreach (var restaurant in restaurants)
        {
            var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var restaurantController = new RestaurantController(mediator, _logger);
            var menuController = new MenuController(mediator);
            i++;
            var result = await restaurantController.CreateRestaurantAsync(restaurant, CancellationToken.None);
            var okResult = result as OkObjectResult;
            var data = okResult.Value as RestaurantId;
            restaurantsId.Add(data.Value);


            scope.Dispose();
            Console.WriteLine($"Restaurant: {i}");
            Console.WriteLine(DateTime.Now);
        }

        foreach (var restaurantId in restaurantsId)
        {
            var menus = CreateMenus(4, restaurantId, ingredients, categories);
            var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var menuController = new MenuController(mediator);
            
            await menuController.CreateMenuAsync(new CreateMenusRequest(menus), CancellationToken.None);
            scope.Dispose();
            Console.WriteLine($"Menus for Restaurant: {restaurantId}");
        }
    }

    //lodz cooridnates
    // Latitude: ~ 51.65°N to 51.85°N
    //Longitude: ~ 19.30°E to 19.65°E

    private static List<CreateRestaurantRequest> GenerateRestaurants(int n, List<short> openingHours,
        List<short> closingHours)
    {
        //move it 
        var result = new List<CreateRestaurantRequest>(n);

        for (var i = 0; i < n; i++)
        {
            var restaurantAddress = new CreateAddressRequest($"restaurant-seed-street-{i}", $"restaurant-city-seed-{i}",
                GetRandomDouble(Random.Shared, 16.30, 22.65), GetRandomDouble(Random.Shared, 50.65, 53.85));
            var owner = new CreateOwnerRequest($"owner-seed-name-{i}", $"owner-seed-surname-{i}",
                new CreateAddressRequest($"seed-street-{i}", $"city-seed-{i}", Random.Shared.NextDouble(),
                    Random.Shared.NextDouble()));
            var openDays = Enumerable.Range(0, 7)
                .Select(d => new WorkingDayRequest((DayOfWeek)d,
                    new DateTime(2020, 12, 12, openingHours[i % openingHours.Count], 00, 00),
                    new DateTime(2020, 12, 12, closingHours[i % closingHours.Count], 00, 00))).ToList();
            var openinghours = new OpeningHoursRequest(openDays);
            result.Add(new CreateRestaurantRequest($"Data-seed-{i}", owner, openinghours, restaurantAddress));
        }

        return result;
    }

    private static List<CreateMenuRequest> CreateMenus(int n, int restaurantId,
        List<CreateIngredientRequest> ingredients, List<string> categories)
    {
        const int groupsPerMenu = 10;
        const int mealsPerGroup = 25;
        const string menuName = "menu-name-seed-";
        const string groupName = "group-name-seed-";
        const string mealName = "meal-name-seed-";

        var menus = new List<CreateMenuRequest>(n);
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
                        GetRandomNumbersOfElements(ingredients, Random.Shared, 15),
                        GetRandomNumbersOfElements(categories, Random.Shared, 12));

                    meals.Add(meal);
                }

                var group = new CreateGroupRequest(restaurantId + groupName + i + j, [..meals]);
                groups.Add(group);
            }

            menus.Add(new CreateMenuRequest(name, [..groups], restaurantId));
        }

        return menus;
    }

    private static double GetRandomDouble(Random random, double min, double max)
    {
        return min + (random.NextDouble() * (max - min));
    }

    private static List<T> GetRandomNumbersOfElements<T>(List<T> source, Random random, int numberOfElements)
        => [..source.OrderBy(x => random.Next()).Take(Math.Min(numberOfElements, source.Count)).ToList()];
}