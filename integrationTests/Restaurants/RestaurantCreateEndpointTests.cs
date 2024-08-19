using System.Net;
using System.Net.Http.Json;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using infrastructure.Database.MenuContext;
using integrationTests.Restaurants.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Restaurants
{
    [TestFixture]
    public class RestaurantCreateEndpointTests : BaseRestaurantIntegrationTests
    {
        private const string _endpoint = "/api/Restaurant";

        [Test]
        public async Task CreateRestaurant_RequestIsNull_Repsonse()
        {
            var response = await _client.PostAsJsonAsync<CreateRestaurantRequest>(_endpoint, null, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var anyRestaurants = await _dbContext.Restaurants.AnyAsync();
            anyRestaurants.Should().BeFalse();
        }

        [Test]
        public async Task CreateRestaurant_Valid_Created()
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);

            result.Should().NotBeNull();
            result!.Value.Should().BeGreaterThan(0);

            var anyRestaurants = await _dbContext.Restaurants.AnyAsync();
            anyRestaurants.Should().BeTrue();
        }

        [Test]
        public async Task CreateRestaurant_CreatedInMenuContext_Success()
        {
            var menuDbContext = await GetDifferentDbContext<MenuDbContext>();
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);


            var restaurantDb = await menuDbContext.Restaurants
                .Where(r => r.Value == result!.Value)
                .ToListAsync();

            restaurantDb.Count.Should().Be(1);
        }
    }
}
