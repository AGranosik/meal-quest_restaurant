using System.Net;
using System.Net.Http.Json;
using domain.Menus.Aggregates;
using FluentAssertions;
using infrastructure.EventStorage.DatabaseModels;
using integrationTests.Common;
using integrationTests.Menus.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Menus.Requests;
using MenuMenuId = domain.Menus.ValueObjects.Identifiers.MenuId;

namespace integrationTests.Menus;

[TestFixture]
internal class CreateMenuEndpointTests : BaseMenuIntegrationTests
{
    private const string _endpoint = "/api/Menu";

    public CreateMenuEndpointTests() : base([ContainersCreator.Postgres, ContainersCreator.RabbitMq])
    {
    }

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
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(_dbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        var menus = MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId, 10);
        var result = await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint,
            menus[0], CancellationToken.None);

        result.Should().NotBeNull();
        result!.Value.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task CreateMenu_SameCategory_Created()
    {
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(_dbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        const int numberOfCategories = 10;
        var menus = MenuDataFaker.ValidRequests(2, 3, 3, 3, restaurantId, numberOfCategories);
        await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint,
            menus[0], CancellationToken.None);

        var result = await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint,
            menus[1], CancellationToken.None);

        result.Should().NotBeNull();
        result!.Value.Should().BeGreaterThan(0);

        var uniqueCategories = menus.SelectMany(m => m.Groups).SelectMany(g => g.Meals)
            .SelectMany(m => m.Categories).Distinct();

        uniqueCategories.Should().HaveCount(numberOfCategories);
        var dbCategories = await _dbContext.Categories.ToListAsync(TestContext.CurrentContext.CancellationToken);
        dbCategories.Should().HaveCount(numberOfCategories);
    }

    [Test]
    public async Task CreateMenu_CreatedInRestaurantContext_Success()
    {
        var restaurantId = await MenuDataFaker.CreateRestaurantForSystem(_client);

        var request = MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId.Value, 10)[0];
        var result =
            await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint, request, CancellationToken.None);

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
        var restaurantId = await MenuDataFaker.CreateRestaurantForSystem(_client);
        var result = await _client.TestPostAsync<CreateMenuRequest, MenuMenuId>(_endpoint,
            MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId.Value, 10)[0], CancellationToken.None);

        var events = await _eventDbContext.GetDbSet<Menu, MenuMenuId>()
            .Where(e => e.StreamId == result!.Value)
            .ToListAsync();

        events.Count.Should().Be(1);
        var @event = events[0];
        @event.Should().NotBeNull();

        @event.HandlingStatus.Should().Be(HandlingStatus.Propagated);

        @event.Data.Should().BeAssignableTo<Menu>();
    }
}