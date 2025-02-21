using MediatR;
using Microsoft.Extensions.Logging;
using webapi.Controllers.Restaurants;
using webapi.Controllers.Restaurants.Requests;

namespace Restaurant.DataSeed.Seed
{
    public class DataSeed
    {
        // use controller
        private readonly IMediator _mediator;
        private readonly ILogger<RestaurantController> _logger;
        public DataSeed(IMediator mediator, ILogger<RestaurantController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }



        public async Task Seed()
        {
            var controller = new RestaurantController(_mediator, _logger);
            var restaurants = GenerateRestaurants(100);
            foreach(var restaurant in restaurants)
            {
                await controller.CreateRestaurantAsync(restaurant, CancellationToken.None);
            }
        }

        //lodz cooridnates
        // Latitude: ~ 51.65°N to 51.85°N
        //Longitude: ~ 19.30°E to 19.65°E

        private static List<CreateRestaurantRequest> GenerateRestaurants(int n)
        {
            Random rand = new();
            var result = new List<CreateRestaurantRequest>(n);
            var openingHours = new List<short> { 6, 7, 8, 9 };
            var closingHours = new List<short> { 20, 21, 22, 23 };
            for(var i =0; i < n; i++)
            {
                var restaurantAddress = new CreateAddressRequest($"restaurant-seed-street-{i}", $"restaurant-city-seed-{i}", GetRandomDouble(rand, 51.65, 51.85), GetRandomDouble(rand, 19.30, 19.65));
                var owner = new CreateOwnerRequest($"owner-seed-name-{i}", $"owner-seed-surname-{i}", new CreateAddressRequest($"seed-street-{i}", $"city-seed-{i}", rand.NextDouble(), rand.NextDouble()));
                var openDays = Enumerable.Range(0, 7)
                    .Select(d => new WorkingDayRequest((DayOfWeek)d, new DateTime(2020, 12, 12, openingHours[i%openingHours.Count], 00, 00), new DateTime(2020, 12, 12, closingHours[i%closingHours.Count], 00, 00))).ToList();
                var openinghours = new OpeningHoursRequest(openDays);
                result.Add(new CreateRestaurantRequest($"Data-seed-{i}", owner, openinghours, restaurantAddress));


            }

            return result;
        }

        static double GetRandomDouble(Random random, double min, double max)
        {
            return min + (random.NextDouble() * (max - min));
        }
    }


}
