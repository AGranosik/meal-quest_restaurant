using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;
using domain.Restaurants.Aggregates.DomainEvents;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates
{
    // add restaurant name
    public class Restaurant: Aggregate<RestaurantId>
    {
        protected List<Menu> _menus = new();
        public IReadOnlyCollection<Menu> Menus => _menus.AsReadOnly();

        protected Restaurant() : base() { }
        public Owner Owner { get; protected set; }
        public OpeningHours OpeningHours { get; protected set; }
        public static Result<Restaurant> Create(Owner owner, OpeningHours openingHours)
        {
            var creationResult = CreationValidation(owner, openingHours);
            if (creationResult.IsFailed)
                return creationResult;

            
            return Result.Ok(new Restaurant(owner, openingHours));
        }

        public Result AddMenu(Menu menu)
        {
            if (_menus.Contains(menu))
                return Result.Fail("Menu already at restaurant.");

            _menus.Add(menu);
            return Result.Ok();
        }

        protected Restaurant(Owner owner, OpeningHours openingHours)
        {
            Owner = owner;
            OpeningHours = openingHours;
            _domainEvents.Add(new RestaurantCreatedEvent(Id!));
        }

        private static Result CreationValidation(Owner owner, OpeningHours openingHours)
        {
            if (owner is null)
                return Result.Fail("Owner cannot be null.");

            if (openingHours is null)
                return Result.Fail("Opening houts cannot be null.");

            return Result.Ok();
        }

        //Tests
        public override List<DomainEvent> GetEvents()
        {
            if (Id is null)
                return _domainEvents;

            foreach(var @event in _domainEvents)
            {
                @event.SetId(Id!.Value);
            }

            return _domainEvents;
        }
    }
}
