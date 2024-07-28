using System.Net.Http.Json;
using FluentAssertions;
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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        }
    }
}
