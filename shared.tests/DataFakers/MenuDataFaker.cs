using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;

namespace sharedTests.DataFakers;

public static class MenuDataFaker
{
    public const int RestaurantId = 2;

    public static Menu ValidMenu(bool isActive = true, int menuId = 1)
    {
        var menu = Menu.Create(ValidGroups(), ValidName, ValidRestaurant, isActive).Value;
        menu.SetId(new MenuId(menuId));
        return menu;
    }

    public static Name ValidName
        => new("test");

    public static List<Category> ValidCategories
        => [new Category("Restaurant"), new Category("Meal")];
    
    // fix creation
    public static List<Group> ValidGroups()
    {
        var categories = new List<Category>
            { new Category("cat1"), new Category("cat2"), new Category("cat3") };
        
      return [
         Group.Create(
         [
             new(
             [
                 Ingredient.Create("test").Value
             ],
             new List<Category>(){categories[0],  categories[1]}
             , new Price(20), new Name("test"))
         ], new Name("hehe")).Value,
         Group.Create(
         [
             new(
             [
                 Ingredient.Create("test2").Value
             ],
             new List<Category>(){categories[2],  categories[1]},
             new Price(20), new Name("test2"))
         ], new Name("hehe2")).Value
     ];   
    }

    public static MenuRestaurant ValidRestaurant
        => new(new RestaurantIdMenuId(RestaurantId));
}