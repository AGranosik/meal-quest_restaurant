using System.Net.Http.Json;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.ValueObjects.Identifiers;
using infrastructure.Database.MenuContext;
using integrationTests.Restaurants.DataMocks;
using webapi.Controllers.Menus.Requests;

namespace integrationTests.Menus.DataMocks
{
    internal static class MenuDataFaker
    {
        internal static List<CreateMenuRequest> ValidRequests(int numberOfMenus, int numberOfGroupsPerMenu, int numberOfMealsPerGroup, int numberOfIngredientsPerMeal, int restaurantId)
        {
            var result = new List<CreateMenuRequest>(numberOfMenus);
            for(int m = 0; m < numberOfMenus; m++)
            {
                var groups = new List<CreateGroupRequest>(numberOfGroupsPerMenu);

                for(int g = 0; g < numberOfGroupsPerMenu; g++)
                {
                    var meals = new List<CreateMealRequest>(numberOfMealsPerGroup);

                    for(int mealI = 0; mealI < numberOfMealsPerGroup; mealI++)
                    {
                        var ingredients = new List<CreateIngredientRequest>(numberOfIngredientsPerMeal);
                        for(int i  = 0; i < numberOfIngredientsPerMeal; i++)
                        {
                            ingredients.Add(new CreateIngredientRequest("ingredient" + i));
                        }
                        meals.Add(new CreateMealRequest("meal" + mealI, mealI + 1, ingredients));
                    }

                    groups.Add(new CreateGroupRequest("group" + g, meals));
                }
                result.Add(new CreateMenuRequest("Menu" + m, groups, restaurantId));
            }

            return result;
        }

        internal static List<RestaurantIdMenuId> Restaurants(int numberOfRestaurants)
        {
            var result = new List<RestaurantIdMenuId>(numberOfRestaurants);
            for(int i = 1; i <= numberOfRestaurants; i++)
            {
                result.Add(new RestaurantIdMenuId(i));
            }
            return result;
        }

        internal static async Task<RestaurantId> CreateRestaurantForSystem(HttpClient client)
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await client.PostAsJsonAsync("/api/Restaurant", request, CancellationToken.None);
            var resultString = await response.Content.ReadAsStringAsync();
            return ApiResponseDeserializator.Deserialize<RestaurantId>(resultString)!;
        }

        internal static async Task<List<RestaurantIdMenuId>> CreateRestaurantsAsync(MenuDbContext dbContext, int numberOfRestaurants)
        {
            var restaurants = MenuDataFaker.Restaurants(numberOfRestaurants);
            dbContext.Restaurants.AddRange(restaurants);
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();
            return restaurants;
        }
    }
}
