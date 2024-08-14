using System.Net;
using System.Text.Json;
using application.Restaurants.Queries.GetRestaurantQueries.Dtos;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace integrationTests.Restaurants
{
    [TestFixture]
    public class GetRestaurantEndpointTests : BaseRestaurantIntegrationTests
    {
        [Test]
        public async Task GetRestaurant_NoneExist_EmptyList()
        {
            var response = await _client.GetAsync("/api/Restaurant?ownerId=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<RestaurantDto>>(resultString);
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetRestaurant_NoneWithProvidedId_EmptyList()
        {
            var restaurants = await AddRestaurants(1, 1);
            var maxId = MaxId(restaurants) + 1;

            var response = await _client.GetAsync($@"/api/Restaurant?ownerId={maxId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<RestaurantDto>>(resultString);
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetRestaurant_OneInDatabase_SingleElement()
        {
            var restaurants = await AddRestaurants(1, 1);
            var maxId = MaxId(restaurants);

            var response = await _client.GetAsync($@"/api/Restaurant?ownerId={maxId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = resultString.Deserialize<List<RestaurantDto>>();

            result.Should().NotBeEmpty();
            result!.Count.Should().Be(1);

            CompareRestaurant(result.First(), restaurants.First())
                .Should().BeTrue();
        }

        [Test]
        public async Task GetRestaurant_MultipleInDatabase_SingleElement()
        {
            var restaurants = await AddRestaurants(10, 1);
            var restaurantToGet = restaurants[8];

            var response = await _client.GetAsync($@"/api/Restaurant?ownerId={restaurantToGet.Owner.Id!.Value}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = resultString.Deserialize<List<RestaurantDto>>();

            result.Should().NotBeEmpty();
            result!.Count.Should().Be(1);

            CompareRestaurant(result.First(), restaurantToGet)
                .Should().BeTrue();
        }

        [Test]
        public async Task GetRestaurant_MultipleInDatabase_MultipleElements()
        {
            var restaurants = await AddRestaurants(12, 3);
            var restaurantToGet = restaurants[8];
            var ownerId = restaurantToGet.Owner.Id!.Value;

            var response = await _client.GetAsync($@"/api/Restaurant?ownerId={ownerId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = resultString.Deserialize<List<RestaurantDto>>();

            var ownersRestaurants = restaurants.Where(r => r.Owner.Id!.Value == ownerId).ToList();
            ownersRestaurants.Count().Should().Be(result!.Count);

            CompareRestaurants(result, ownersRestaurants);
        }

        //move it to faker
        private async Task<List<Restaurant>> AddRestaurants(int numberOfRestaurants, int restaurantsPerOwner)
        {
            var restaurants = new List<Restaurant>(numberOfRestaurants);

            int iRestaurant = 0;
            while (iRestaurant < numberOfRestaurants)
            {
                int iOwner = 0;
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

                    var restaurant = Restaurant.Create(owner, openingHours);
                    restaurants.Add(restaurant.Value);
                    iOwner++;
                    iRestaurant++;
                }
            }
            _dbContext.Restaurants.AddRange(restaurants);
            await _dbContext.SaveChangesAsync();
            return restaurants;
        }

        private static bool CompareRestaurants(List<RestaurantDto> dtos, List<Restaurant> domains)
        {
            foreach(var dto in dtos)
            {
                var domain = domains.FirstOrDefault(d => d.Id.Value == dto.Id);
                if (domain is null)
                    return false;

                var equal = CompareRestaurant(dto, domain);
                if (!equal)
                    return false;
            }

            return true;
        }

        private static bool CompareRestaurant(RestaurantDto dto, Restaurant domain)
        {
            if (dto.Id != domain.Id!.Value)
                return false;

            if(dto.Owner.Name != domain.Owner.Name.Value
                || dto!.Owner!.Surname != domain.Owner.Surname.Value) return false;

            var dtoAddress = dto.Owner.Address;
            var domainAddress = domain.Owner.Address;

            if(dtoAddress.City != domainAddress.City.Value
                || dtoAddress.XCoordinate != domainAddress.Coordinates.X
                || dtoAddress.YCoordinate != domainAddress.Coordinates.Y)
                return false;

            if(!dto.OpeningHours.WorkingDays.Any(dtoWd => domain.OpeningHours.WorkingDays.Any(domainWWd => dtoWd.Day == domainWWd.Day))
                || dto.OpeningHours.WorkingDays.Count != domain.OpeningHours.WorkingDays.Count)
                return false;


            return true;
        }

        private static int MaxId(List<Restaurant> restaurants)
            => restaurants.Select(r => r.Owner.Id!.Value).Max();
    }
}
