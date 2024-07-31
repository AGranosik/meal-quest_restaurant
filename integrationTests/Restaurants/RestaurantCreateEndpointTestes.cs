using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using integrationTests.Restaurants.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Restaurants
{
    [TestFixture]
    public class RestaurantCreateEndpointTestes : BaseContainerIntegrationTests
    {
        [Test]
        public async Task CreateRestaurant_RequestIsNull_Repsonse()
        {
            var response = await _client.PostAsJsonAsync<CreateRestaurantRequest>("/api/Restaurant", null, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var empty = await _dbContext.Restaurants.AnyAsync();
            empty.Should().BeFalse();
        }

        [Test]
        public async Task CreateRestaurant_Valid_Created()
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync("/api/Restaurant", request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var empty = await _dbContext.Restaurants.AnyAsync();
            empty.Should().BeTrue();
        }
    }
}
