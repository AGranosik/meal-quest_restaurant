using System.Net.Http.Json;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.ValueObjects.Identifiers;
using infrastructure.Database.MenuContext;
using integrationTests.Common;
using integrationTests.Restaurants.DataMocks;
using webapi.Controllers.Menus.Requests;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Menus.DataMocks;

internal static class MenuDataFaker
{
    internal static CreateMenusRequest ValidRequests(int numberOfMenus, int numberOfGroupsPerMenu, int numberOfMealsPerGroup, int numberOfIngredientsPerMeal, int restaurantId, int numberOfCategories)
    {
        var result = new List<CreateMenuRequest>(numberOfMenus);
        for(var m = 0; m < numberOfMenus; m++)
        {
            var groups = new List<CreateGroupRequest>(numberOfGroupsPerMenu);

            for(var g = 0; g < numberOfGroupsPerMenu; g++)
            {
                var meals = new List<CreateMealRequest>(numberOfMealsPerGroup);

                for(var mealI = 0; mealI < numberOfMealsPerGroup; mealI++)
                {
                    var ingredients = new List<CreateIngredientRequest>(numberOfIngredientsPerMeal);
                    for(var i  = 0; i < numberOfIngredientsPerMeal; i++)
                    {
                        ingredients.Add(new CreateIngredientRequest("ingredient" + i));
                    }

                    var categories = new List<string>(numberOfCategories);
                    for (var i = 0; i < numberOfCategories; i++)
                    {
                        categories.Add(i.ToString());
                    }
                    meals.Add(new CreateMealRequest("meal" + mealI, mealI + 1, ingredients, categories));
                }

                groups.Add(new CreateGroupRequest("group" + g, meals));
            }
            
            result.Add(new CreateMenuRequest("Menu" + m, groups, restaurantId));
        }

        return new(result);
    }

    private static List<MenuRestaurant> Restaurants(int numberOfRestaurants)
    {
        var result = new List<MenuRestaurant>(numberOfRestaurants);
        for(var i = 1; i <= numberOfRestaurants; i++)
        {
            result.Add(new MenuRestaurant(new RestaurantIdMenuId(i)));
        }
        return result;
    }

    internal static Task<RestaurantId?> CreateRestaurantForSystem(HttpClient client)
    {
        var request = RestaurantDataFaker.ValidRequest();

        return client.TestPostMultipartForm<CreateRestaurantRequest, RestaurantId>("/api/Restaurant", request, TestContext.CurrentContext.CancellationToken);
    }

    internal static async Task<List<MenuRestaurant>> CreateRestaurantsAsync(MenuDbContext dbContext, int numberOfRestaurants)
    {
        var restaurants = Restaurants(numberOfRestaurants);
        dbContext.Restaurants.AddRange(restaurants);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
        return restaurants;
    }
}