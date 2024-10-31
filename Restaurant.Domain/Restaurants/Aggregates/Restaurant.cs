using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates
{
    public class Restaurant: Aggregate<RestaurantId>
    {
        protected List<Menu> _menus = new();
        public IReadOnlyCollection<Menu> Menus => _menus.AsReadOnly();

        public Name Name { get; protected set; }
        public Owner Owner { get; protected set; }
        public OpeningHours OpeningHours { get; protected set; }
        public static Result<Restaurant> Create(Name name, Owner owner, OpeningHours openingHours)
        {
            var creationResult = CreationValidation(name, owner, openingHours);
            if (creationResult.IsFailed)
                return creationResult;

            return Result.Ok(new Restaurant(name, owner, openingHours));
        }

        public Result AddMenu(Menu menu)
        {
            if (_menus.Contains(menu))
                return Result.Fail("Menu already at restaurant.");

            _menus.Add(menu);
            return Result.Ok();
        }
        protected Restaurant() : base() { }

        private Restaurant(Name name, Owner owner, OpeningHours openingHours)
        {
            Owner = owner;
            OpeningHours = openingHours;
            Name = name;
        }

        private static Result CreationValidation(Name name, Owner owner, OpeningHours openingHours)
        {
            if (name is null)
                return Result.Fail("Name cannot be null.");

            if (owner is null)
                return Result.Fail("Owner cannot be null.");

            if (openingHours is null)
                return Result.Fail("Opening houts cannot be null.");

            return Result.Ok();
        }
    }
}
