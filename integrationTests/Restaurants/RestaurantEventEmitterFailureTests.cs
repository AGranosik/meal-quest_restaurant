using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using infrastructure.EventStorage.DatabaseModels;
using integrationTests.Common;
using integrationTests.Restaurants.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Restaurants;

[TestFixture]
internal class RestaurantEventEmitterFailureTests : BaseRestaurantIntegrationTests
{
    private const string _endpoint = "/api/Restaurant";
    public RestaurantEventEmitterFailureTests() : base([ContainersCreator.Postgres])
    {
    }

    [Test]
    public async Task CreateRestaurant_ServiceBusIsNotAvailable_RestaurantSaved()
    {
        var request = RestaurantDataFaker.ValidRequest();

        var result = await Client.TestPostAsync<CreateRestaurantRequest, RestaurantId>(_endpoint, request, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Value.Should().BeGreaterThan(0);

        var anyRestaurants = await DbContext.Restaurants.AnyAsync();
        anyRestaurants.Should().BeTrue();
    }

    [Test]
    public async Task CreateRestaurant_Valid_StoredInEventStore()
    {
        var request = RestaurantDataFaker.ValidRequest();

        var result = await Client.TestPostAsync<CreateRestaurantRequest, RestaurantId>(_endpoint, request, CancellationToken.None);

        var events = await _eventDbContext.GetDbSet<Restaurant, RestaurantId>()
            .Where(e => e.StreamId == result!.Value)
            .ToListAsync();

        events.Count.Should().Be(1);

        events[0].Data.Should().BeAssignableTo<Restaurant>();
    }

    [Test]
    public async Task CreateRestaurant_Valid_StoredFailureInEventStore()
    {
        var request = RestaurantDataFaker.ValidRequest();

        var result = await Client.TestPostAsync<CreateRestaurantRequest, RestaurantId>(_endpoint, request, CancellationToken.None);

        var events = await _eventDbContext.GetDbSet<Restaurant, RestaurantId>()
            .Where(e => e.StreamId == result!.Value)
            .ToListAsync();


        var @event = events[0];
        @event.HandlingStatus.Should().Be(HandlingStatus.Failed);
    }
}