using System.Data.Common;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using FluentResults;
using integrationTests.Restaurants.DataMocks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Restaurants
{
    [TestFixture]
    public class RestaurantCreateEndpointTests : BaseContainerIntegrationTests
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

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result<RestaurantId>>(resultString);

            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBe(0);

            var empty = await _dbContext.Restaurants.AnyAsync();
            empty.Should().BeTrue();
        }

        protected override async Task OneTimeSetUp()
        {
            await base.OneTimeSetUp();
            var connection = _dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToInclude =
                [
                    new Table("restaurant", "WorkingDays"),
                    new Table("restaurant", "Restaurants"),
                    new Table("restaurant", "OpeningHours"),
                    new Table("restaurant", "Addresses"),
                    new Table("restaurant", "Owners"),
                ],
                SchemasToInclude = new[] 
                {
                    "public",
                    "restaurant"
                } 
            });
        }
    }
}
