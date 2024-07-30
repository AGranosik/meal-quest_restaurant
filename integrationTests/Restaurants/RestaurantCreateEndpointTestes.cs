using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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
            var openingHours = new OpeningHoursRequest(
            [
                new(DayOfWeek.Monday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Tuesday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Wednesday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Thursday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Friday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Saturday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Sunday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
            ]);
            var address = new CreateAddressRequest("street", "City", 0, 0);
            var owner = new CreateOwnerRequest("name", "surname", address);
            var request = new CreateRestaurantRequest(owner, openingHours);

            var response = await _client.PostAsJsonAsync("/api/Restaurant", request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var empty = await _dbContext.Restaurants.AnyAsync();
            empty.Should().BeTrue();
        }
    }
}
