using FluentResults;
using core.Extensions;
using domain.Common.BaseTypes;
using domain.Menus.ValueObjects.Identifiers;
using domain.Menus.ValueObjects;
using domain.Menus.Aggregates.DomainEvents;
using domain.Common.ValueTypes.Strings;

namespace domain.Menus.Aggregates.Entities
{
    public sealed class Menu : Entity<MenuId>
    {
        public static Result<Menu> Create(List<Group> groups, Name name, RestaurantIdMenuId restaurant)
        {
            var validatioNResult = CreationValidation(groups, restaurant);
            if (validatioNResult.IsFailed)
                return validatioNResult;

            var menu = new Menu(groups, name, restaurant);
            return Result.Ok(menu);
        }
        private Menu() { }
        public Name Name { get; }
        public List<Group> Groups { get; }
        public RestaurantIdMenuId Restaurant { get; }
        private Menu(List<Group> groups, Name name, RestaurantIdMenuId restaurant)
        {
            Groups = groups;
            Name = name;
            Restaurant = restaurant;
            _domainEvents.Add(new MenuCreatedEvent(this));
        }

        private static Result CreationValidation(List<Group> groups, RestaurantIdMenuId restaurantId)
        {
            if (groups is null || groups.Count == 0)
                return Result.Fail("Groups are missing.");

            if (!groups.HasUniqueValues())
                return Result.Fail("Groups has to be unique.");

            if (restaurantId is null)
                return Result.Fail("Restaurant id cannot be null.");

            return Result.Ok();
        }
    }
}
