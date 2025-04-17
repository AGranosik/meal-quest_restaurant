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
internal class MenuEventEmitterFailureTests : BaseMenuIntegrationTests
{
    private const string _endpoint = "/api/Menu";
    public MenuEventEmitterFailureTests() : base([ContainersCreator.Postgres])
    {
    }

    [Test]
    public async Task CreateMenu_ServiceBusIsNotAvailable_MenuSaved()
    {
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(DbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        var result = await Client.TestPostAsync<CreateMenusRequest, List<MenuMenuId>>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId, 10), CancellationToken.None);

        result.Should().NotBeNull();
        result![0].Value.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task CreateMenu_Valid_StoredInEventStore()
    {
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(DbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        var result = await Client.TestPostAsync<CreateMenusRequest, List<MenuMenuId>>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId, 10), CancellationToken.None);

        var events = await EventDbContext.GetDbSet<Menu, MenuMenuId>()
            .Where(e => e.StreamId == result![0].Value)
            .ToListAsync();

        events.Count.Should().Be(1);
        events[0].Data.Should().BeAssignableTo<Menu>();
    }

    //TODO: TEST MULTIPLE MENUS
    //TODO: REPLACE RESULT[0] WITH SOME METHODS -> JUST REFACTOR
    [Test]
    public async Task CreateMenu_Valid_StoredFailureInEventStore()
    {
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(DbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        var result = await Client.TestPostAsync<CreateMenusRequest, List<MenuMenuId>>(_endpoint, MenuDataFaker.ValidRequests(1, 3, 3, 3, restaurantId, 10), CancellationToken.None);

        var events = await EventDbContext.GetDbSet<Menu, MenuMenuId>()
            .Where(e => e.StreamId == result![0].Value)
            .ToListAsync();

        events.Count.Should().Be(1);
        var @event = events[0];
        @event.HandlingStatus.Should().Be(HandlingStatus.Failed);
    }
}