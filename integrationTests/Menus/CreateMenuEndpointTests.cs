﻿using System.Net;
using System.Net.Http.Json;
using domain.Menus.Aggregates.DomainEvents;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using infrastructure.EventStorage.DatabaseModels;
using integrationTests.Common;
using integrationTests.Menus.DataMocks;
using integrationTests.Restaurants.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Menus.Requests;
using webapi.Controllers.Restaurants.Requests;

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
            var restaurantId = restaurants.First().Value;
            var result = await _client.TestPostAsync<CreateMenuRequest, domain.Menus.ValueObjects.Identifiers.MenuId>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId).First(), CancellationToken.None);

            result.Should().NotBeNull();
            result!.Value.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task CreateMenu_CreatedInRestaurantContext_Success()
        {
            var restaurantId = await CreateRestaurantForSystem();

            var request = MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId.Value).First();
            var result = await _client.TestPostAsync<CreateMenuRequest, domain.Menus.ValueObjects.Identifiers.MenuId>(_endpoint, request, CancellationToken.None);

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
            var result = await _client.TestPostAsync<CreateMenuRequest, domain.Menus.ValueObjects.Identifiers.MenuId>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId.Value).First(), CancellationToken.None);

            var events = await _eventDbContext.GetDbSet<MenuEvent>()
                .Where(e => e.StreamId == result!.Value)
                .ToListAsync();

            events.Count.Should().Be(1);
            var @event = events.First();

            @event.PropgationStatus.Should().Be(EventProapgationStatus.Propagated);

            @event.Data.Should().BeAssignableTo<MenuCreatedEvent>();
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
