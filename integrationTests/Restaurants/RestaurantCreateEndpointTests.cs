using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using FluentResults;
using infrastructure.Database.RestaurantContext;
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
            var result = JsonSerializer.Deserialize<Result<RestaurantId>>(resultString);

            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBe(0);

            var anyRestaurants = await _dbContext.Restaurants.AnyAsync();
            anyRestaurants.Should().BeTrue();
        }


    }
}
