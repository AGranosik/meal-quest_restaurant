using System.Net;
using System.Net.Http.Json;
using domain.Restaurants.Aggregates.DomainEvents;
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
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);


            var menuDb = await _menuDbContext.Restaurants
                .Where(r => r.Value == result!.Value)
                .ToListAsync();

            menuDb.Count.Should().Be(1);
        }

        [Test]
        public async Task CreateRestaurant_Valid_StoredInEventStore()
        {
            var request = RestaurantDataFaker.ValidRequest();

            //mvoe to method to extract response from request
            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);


            var events = await _eventDobContext.GetDbSet<RestaurantEvent>()
                .Where(e => e.StreamId == result!.Value)
                .ToListAsync();

            events.Count.Should().Be(1);
        }

    }
}
