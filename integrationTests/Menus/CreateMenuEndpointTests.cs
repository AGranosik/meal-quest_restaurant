using System.Net;
using System.Net.Http.Json;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using infrastructure.EventStorage.DatabaseModels;
using integrationTests.Common;
using integrationTests.Menus.DataMocks;
using integrationTests.Restaurants.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Menus.Requests;
using MenuMenuId = domain.Menus.ValueObjects.Identifiers.MenuId;

namespace integrationTests.Menus
{
    [TestFixture]
    internal class CreateMenuEndpointTests : BaseMenuIntegrationTests
    {
        private const string _endpoint = "/api/Menu";

        [Test]
        public async Task CreateMenu_RequestIsNull_BadRequest()
        {
            var response = await _client.PostAsJsonAsync<CreateMenuRequest?>(_endpoint, null, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var anyMenus = await _dbContext.Menus.AnyAsync();
            anyMenus.Should().BeFalse();
        }

        [Test]
        public async Task CreateMenu_Valid_Created()
        {
            var restaurants = await CreateRestaurantsAsync(1);
            var restaurantId = restaurants[0].Value;
            var result = await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId)[0], CancellationToken.None);

            result.Should().NotBeNull();
            result!.Value.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task CreateMenu_CreatedInRestaurantContext_Success()
        {
            var restaurantId = await CreateRestaurantForSystem();

            var request = MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId.Value)[0];
            var result = await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint, request, CancellationToken.None);

            var dbRestaurant = await _restaurantDbContext.Restaurants
                .Include(r => r.Menus)
                .Include(r => r.Owner)
                .Include(r => r.OpeningHours)
                    .ThenInclude(oh => oh.WorkingDays)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id! == restaurantId);

            dbRestaurant.Should().NotBeNull();

            dbRestaurant!.Menus.Count.Should().Be(1);
            var restaurantDbMenu = dbRestaurant.Menus.First();

            (restaurantDbMenu.Id!.Value == result!.Value).Should().BeTrue();
            (restaurantDbMenu.Name.Value == request.Name!).Should().BeTrue();
        }

        [Test]
        public async Task CreateMenu_Valid_EventStoredInEventStore()
        {
            var restaurantId = await CreateRestaurantForSystem();
            var result = await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId.Value)[0], CancellationToken.None);

            var events = await _eventDbContext.GetDbSet<Menu, MenuMenuId>()
                .Where(e => e.StreamId == result!.Value)
                .ToListAsync();

            events.Count.Should().Be(1);
            var @event = events[0];
            @event.Should().NotBeNull();

            @event.HandlingStatus.Should().Be(HandlingStatus.Propagated);

            @event.Data.Should().BeAssignableTo<Menu>();
        }

        private async Task<RestaurantId> CreateRestaurantForSystem()
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync("/api/Restaurant", request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            return ApiResponseDeserializator.Deserialize<RestaurantId>(resultString)!;
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
