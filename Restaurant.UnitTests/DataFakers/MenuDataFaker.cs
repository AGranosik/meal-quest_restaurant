using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;

namespace unitTests.DataFakers
{
    public static class MenuDataFaker
    {
        public const int RESTAURANT_ID = 2;
        public const int MENU_ID = 1;

        public static Menu ValidMenu()
        {
            var menu = Menu.Create(ValidGroups, ValidName, ValidRestaurant).Value;
            menu.SetId(new MenuId(MENU_ID));
            return menu;
        }

        public static Name ValidName
            => new("test");

        public static List<Group> ValidGroups
            =>
            [
                Group.Create(
                [
                    new(
                    [
                        Ingredient.Create("test").Value
                    ], new Price(20), new Name("test"))
                ], new Name("hehe")).Value,
                Group.Create(
                [
                    new(
                    [
                        Ingredient.Create("test2").Value
                    ], new Price(20), new Name("test2"))
                ], new Name("hehe2")).Value
            ];

        public static RestaurantIdMenuId ValidRestaurant
            => new(RESTAURANT_ID);
    }
}
