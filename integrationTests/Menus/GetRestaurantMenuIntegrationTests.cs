using System.Net;
using application.Menus.Queries.Dto;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using FluentAssertions;
using integrationTests.Common;
using integrationTests.Menus.DataMocks;
using webapi.Controllers.Menus.Requests;
using MenuMenuId = domain.Menus.ValueObjects.Identifiers.MenuId;

namespace integrationTests.Menus;

[TestFixture]
internal sealed class GetRestaurantMenuIntegrationTests : BaseMenuIntegrationTests
{
    private const string ENDPOINT = "/api/Menu?restaurantId=";
    public GetRestaurantMenuIntegrationTests() : base([ContainersCreator.Postgres, ContainersCreator.RabbitMq])
    {
    }

    [Test]
    public async Task GetMenu_NoneInDb_badRequest()
    {
        var response = await Client.GetAsync(ENDPOINT + "1");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetMenu_RestaurantDoesntExistsIntDb_BadRequest()
    {
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(DbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        var menu = MenuDataFaker.ValidRequests(3, 3, 3, restaurantId, 10);
        await Client.TestPostAsync<CreateMenuRequest, MenuMenuId>(ENDPOINT,
            menu, TestContext.CurrentContext.CancellationToken);
        var response = await Client.GetAsync($"{ENDPOINT}{restaurantId + 1}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task GetMenu_SingleElementInDb_Success()
    {
        var restaurants = await MenuDataFaker.CreateRestaurantsAsync(DbContext, 1);
        var restaurantId = restaurants[0].Id!.Value;
        var menu = MenuDataFaker.ValidRequests(3, 3, 3, restaurantId, 10);
        await Client.TestPostAsync<CreateMenuRequest, MenuMenuId>(ENDPOINT,
            menu, TestContext.CurrentContext.CancellationToken);
        var response = await Client.GetAsync($"{ENDPOINT}{restaurantId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultString = await response.Content.ReadAsStringAsync();
        var result = resultString.Deserialize<MenuRestaurantDto>();

        Compare(result!, menu);
    }
    
    private static bool Compare(MenuRestaurantDto dto, CreateMenuRequest request)
    {
        return CompareName(dto.Name, request.Name!)
               && CompareGroups(dto.Groups, request.Groups);
    }
    private static bool CompareName(string dtoName, string requestName) =>
        string.Equals(dtoName, requestName, StringComparison.OrdinalIgnoreCase);

    private static bool CompareGroups(List<MenuGroupDto> dtoGroups, List<CreateGroupRequest> requestGroups)
    {
        if (dtoGroups.Count != requestGroups.Count)
            return false;

        foreach (var dtoGroup in dtoGroups)
        {
            var domainGroup = requestGroups
                .FirstOrDefault(g => NamesEqual(g.Name, dtoGroup.Name));

            if (domainGroup == null || !CompareMeals(dtoGroup.Meals, domainGroup.Meals))
                return false;
        }

        return true;
    }

    private static bool CompareMeals(List<MealDto> dtoMeals, List<CreateMealRequest> requestMeals)
    {
        if (dtoMeals.Count != requestMeals.Count)
            return false;

        foreach (var dtoMeal in dtoMeals)
        {
            var requestMeal = requestMeals
                .FirstOrDefault(m => NamesEqual(m.Name, dtoMeal.Name));

            if (requestMeal == null)
                return false;

            if (!ComparePrice(dtoMeal.Price, requestMeal.Price))
                return false;

            if (!CompareLists(dtoMeal.CategoriesName, 
                              requestMeal.Categories))
                return false;

            if (!CompareLists(dtoMeal.Ingredients.Select(i => i.Name), 
                              requestMeal.Ingredients?.Select(i => i.Name) ?? Enumerable.Empty<string>()))
                return false;
        }

        return true;
    }

    private static bool ComparePrice(decimal dtoPrice, decimal? requestPrice) =>
        dtoPrice == requestPrice;

    private static bool CompareLists(IEnumerable<string> dtoList, IEnumerable<string> domainList)
    {
        var dtoSorted = dtoList.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
        var domainSorted = domainList.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
        return dtoSorted.SequenceEqual(domainSorted, StringComparer.OrdinalIgnoreCase);
    }

    private static bool NamesEqual(string? a, string? b) =>
        string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
}