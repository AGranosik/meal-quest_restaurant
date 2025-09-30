using System.Net;
using System.Text.Json;
using application.Restaurants.Queries.OwnerRestaurantQueries.Dto;
using application.Restaurants.Queries.RestaurantDetailsQueries.Dto;
using domain.Restaurants.Aggregates;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using integrationTests.Common;
using integrationTests.Restaurants.DataMocks;

namespace integrationTests.Restaurants;

[TestFixture]
internal sealed class RestaurantDetailsIntegrationTests : BaseRestaurantIntegrationTests
{
    private const string Endpoint = "/api/Restaurant?restaurantId=";
    public RestaurantDetailsIntegrationTests() : base([ContainersCreator.Postgres, ContainersCreator.RabbitMq])
    {
    }
    
    [Test]
    public async Task GetRestaurant_NoneExist_EmptyList()
    {
        var response = await Client.GetAsync(Endpoint + "1");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task GetRestaurant_NoneWithProvidedId_EmptyList()
    {
        var restaurants = await RestaurantDataFaker.AddRestaurants(1, 1, DbContext, TestContext.CurrentContext.CancellationToken);
        var maxId = MaxId(restaurants) + 1;

        var response = await Client.GetAsync($"{Endpoint}{maxId}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task GetRestaurant_OneInDatabase_SingleElement()
    {
        var restaurants = await RestaurantDataFaker.AddRestaurants(1, 1, DbContext, TestContext.CurrentContext.CancellationToken);
        var maxId = MaxId(restaurants);

        var response = await Client.GetAsync($"{Endpoint}{maxId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = resultString.Deserialize<RestaurantDetailsDto>();

        result.Should().NotBeNull();

        CompareRestaurant(result!, restaurants[0])
            .Should().BeTrue();
    }
    
    [Test]
    public async Task GetRestaurant_MultipleInDatabase_MultipleElements()
    {
        const int numberOfRestaurants = 12;
        var restaurants = await RestaurantDataFaker.AddRestaurants(numberOfRestaurants, 3, DbContext, TestContext.CurrentContext.CancellationToken);
        var elementToPick = (int)(DateTime.Now.Ticks % numberOfRestaurants);
        var restaurantToGet = restaurants[elementToPick];

        var response = await Client.GetAsync($"{Endpoint}{restaurantToGet.Id!.Value}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = resultString.Deserialize<RestaurantDetailsDto>();

        var domainRestaurant = restaurants.ElementAt(elementToPick);

        var comparisonResult = CompareRestaurant(result!, domainRestaurant);
        comparisonResult.Should().BeTrue();
    }
    
    private static bool CompareRestaurant(RestaurantDetailsDto dto, Restaurant domain)
    {
        if (dto.Id != domain.Id!.Value)
            return false;

        var dtoAddress = dto.Address;
        var domainAddress = domain.Address;

        if(dtoAddress.City != domainAddress.City!.Value
           || dtoAddress.XCoordinate != domainAddress.Coordinates!.X
           || dtoAddress.YCoordinate != domainAddress.Coordinates.Y)
            return false;

        if(!dto.OpeningHours.WorkingDays.Exists(dtoWd => domain.OpeningHours.WorkingDays.Any(domainWWd => dtoWd.Day == domainWWd.Day))
           && dto.OpeningHours.WorkingDays.Count == domain.OpeningHours.WorkingDays.Count)
            return false;

        if (string.IsNullOrEmpty(dto.Base64Logo))
            return false;
        
        return dto.Description == domain.Description.Value;
    }
    
    private static int MaxId(List<Restaurant> restaurants)
        => restaurants.Max(r => r.Id!.Value);
}