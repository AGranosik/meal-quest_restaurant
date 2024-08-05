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
    public class GetRestaurantEndpointTests : BaseContainerIntegrationTests
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
            var restaurants = await AddRestaurants();
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
            var restaurants = await AddRestaurants();
            var maxId = MaxId(restaurants);

            var response = await _client.GetAsync($@"/api/Restaurant?ownerId={maxId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<RestaurantDto>>(resultString);

            result.Should().NotBeEmpty();
            result!.Count.Should().Be(1);
        }

        private async Task<List<Restaurant>> AddRestaurants()
        {
            var owner = Owner.Create(new Name("test"), new Name("surname"), Address.Create(new Street("street"), new City("city"), new Coordinates(10, 10)).Value).Value;
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
            _dbContext.Restaurants.Add(restaurant.Value);
            await _dbContext.SaveChangesAsync();
            return
            [
                restaurant.Value
            ];
        }

        private bool CompareRestaurants(Restaurant one, Restaurant two)
        {

        }

        private int MaxId(List<Restaurant> restaurants)
            => restaurants.Select(r => r.Id!.Value).Max();
    }
}
