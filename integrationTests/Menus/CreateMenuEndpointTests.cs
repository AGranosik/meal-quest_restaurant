using System.Net;
using System.Net.Http.Json;
using domain.Menus.ValueObjects.Identifiers;
using FluentAssertions;
using integrationTests.Menus.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Menus.Requests;

namespace integrationTests.Menus
{
    [TestFixture]
    internal class CreateMenuEndpointTests : BaseMenuIntegrationTests
    {
        private const string _endpoint = "/api/Menu";

        [Test]
        public async Task CreateMenu_RequestIsNull_BadRequest()
        {
            var response = await _client.PostAsJsonAsync<CreateMenuRequest>(_endpoint, null, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var anyMenus = await _dbContext.Menus.AnyAsync();
            anyMenus.Should().BeFalse();
        }

        // change PK and FK contraints names
        [Test]
        public async Task CreateMenu_Valid_Created()
        {
            var restaurants = await CreateRestaurantsAsync(1);
            var restaurantId = restaurants.First().Value;
            var response = await _client.PostAsJsonAsync(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId).First(), CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<MenuId>(resultString);

            result.Should().NotBeNull();
            result!.Value.Should().Be(1);
        }

        private async Task<List<RestaurantIdMenuId>> CreateRestaurantsAsync(int numberOfRestaurants)
        {
            var restaurants = MenuDataFaker.Restaurants(numberOfRestaurants);
            _dbContext.Restaurants.AddRange(restaurants);
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();
            return restaurants;
        }
    }
}
