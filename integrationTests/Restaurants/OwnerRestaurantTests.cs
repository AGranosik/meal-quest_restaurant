using System.Net;
using System.Text.Json;
using application.Restaurants.Queries.OwnerRestaurantQueries.Dto;
using domain.Restaurants.Aggregates;
using FluentAssertions;
using integrationTests.Common;
using integrationTests.Restaurants.DataMocks;

namespace integrationTests.Restaurants;

[TestFixture]
internal class OwnerRestaurantTests : BaseRestaurantIntegrationTests
{
    private const string Endpoint = "/api/Restaurant/owner?ownerId=";
    public OwnerRestaurantTests() : base([ContainersCreator.Postgres, ContainersCreator.RabbitMq])
    {
    }

    [Test]
    public async Task GetRestaurant_NoneExist_EmptyList()
    {
        var response = await Client.GetAsync(Endpoint + "1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<OwnerRestaurantDto>>(resultString);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetRestaurant_NoneWithProvidedId_EmptyList()
    {
        var restaurants = await RestaurantDataFaker.AddRestaurants(1, 1, DbContext, TestContext.CurrentContext.CancellationToken);
        var maxId = MaxId(restaurants) + 1;

        var response = await Client.GetAsync($"{Endpoint}{maxId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<OwnerRestaurantDto>>(resultString);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetRestaurant_OneInDatabase_SingleElement()
    {
        var restaurants = await RestaurantDataFaker.AddRestaurants(1, 1, DbContext, TestContext.CurrentContext.CancellationToken);
        var maxId = MaxId(restaurants);

        var response = await Client.GetAsync($"{Endpoint}{maxId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = resultString.Deserialize<List<OwnerRestaurantDto>>();

        result.Should().NotBeEmpty();
        result!.Count.Should().Be(1);

        CompareRestaurant(result[0], restaurants[0])
            .Should().BeTrue();
    }

    [Test]
    public async Task GetRestaurant_MultipleInDatabase_SingleElement()
    {
        var restaurants = await RestaurantDataFaker.AddRestaurants(10, 1, DbContext, TestContext.CurrentContext.CancellationToken);
        var restaurantToGet = restaurants[8];

        var response = await Client.GetAsync($"{Endpoint}{restaurantToGet.Owner.Id!.Value}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = resultString.Deserialize<List<OwnerRestaurantDto>>();

        result.Should().NotBeEmpty();
        result!.Count.Should().Be(1);

        CompareRestaurant(result[0], restaurantToGet)
            .Should().BeTrue();
    }

    [Test]
    public async Task GetRestaurant_MultipleInDatabase_MultipleElements()
    {
        var restaurants = await RestaurantDataFaker.AddRestaurants(12, 3, DbContext, TestContext.CurrentContext.CancellationToken);
        var restaurantToGet = restaurants[8];
        var ownerId = restaurantToGet.Owner.Id!.Value;

        var response = await Client.GetAsync($"{Endpoint}{ownerId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultString = await response.Content.ReadAsStringAsync();
        var result = resultString.Deserialize<List<OwnerRestaurantDto>>();

        var ownersRestaurants = restaurants.Where(r => r.Owner.Id!.Value == ownerId).ToList();
        ownersRestaurants.Count.Should().Be(result!.Count);

        var comparisonResult = CompareRestaurants(result, ownersRestaurants);
        comparisonResult.Should().BeTrue();
    }

    private static bool CompareRestaurants(List<OwnerRestaurantDto> dtos, List<Restaurant> domains)
    {
        foreach(var dto in dtos)
        {
            var domain = domains.Find(d => d.Id!.Value == dto.Id);
            if (domain is null)
                return false;

            var equal = CompareRestaurant(dto, domain);
            if (!equal)
                return false;
        }

        return true;
    }

    private static bool CompareRestaurant(OwnerRestaurantDto dto, Restaurant domain)
    {
        if (dto.Id != domain.Id!.Value)
            return false;

        if(dto.Owner.Name != domain.Owner.Name.Value
           || dto!.Owner!.Surname != domain.Owner.Surname.Value) return false;

        var dtoAddress = dto.Owner.Address;
        var domainAddress = domain.Owner.Address;

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
        => restaurants.Max(r => r.Owner.Id!.Value);
}