using domain.Menus.ValueObjects.Identifiers;
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
    }
}
