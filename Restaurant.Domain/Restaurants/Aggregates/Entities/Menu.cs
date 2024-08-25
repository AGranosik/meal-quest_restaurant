using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;

namespace domain.Restaurants.Aggregates.Entities
{
    public class Menu : Entity<MenuId>
    {
        public Name Name { get; }

        public static Result<Menu> Create(MenuId menuId, Name name)
        {
            var validationResult = Validation(menuId, name);
            if (validationResult.IsFailed)
                return validationResult;

            return Result.Ok(new Menu(menuId, name));
        }

        protected Menu() { }

        private Menu(MenuId menuId, Name name)
        {
            Id = menuId;
            Name = name;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Menu other = obj as Menu;
            if (other == null) return false;
            return Id == other.Id;
        }

        private static Result Validation(MenuId menuId, Name name)
        {
            if (menuId is null)
                return Result.Fail("Menu id cannot be null.");

            if (name is null)
                return Result.Fail("Name cannot be null.");

            return Result.Ok();
        }

        // move to aggregate??
        public override List<DomainEvent> GetEvents()
        {
            throw new NotImplementedException();
        }
    }
}
