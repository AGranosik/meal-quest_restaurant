using FluentResults;
using core.Extensions;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Menus.Aggregates.DomainEvents;
using Restaurant.Domain.Menus.ValueObjects;
using Restaurant.Domain.Menus.ValueObjects.Identifiers;

namespace Restaurant.Domain.Menus.Aggregates.Entities
{
    //available hours
    public sealed class Menu : Entity<MenuId>
    {
        // remove Id from constructors or it should be able to assign nulls? Before db creation
        public static Result<Menu> Create(MenuId id, List<Group> groups)
        {
            var validatioNResult = CreationValidation(groups);
            if (validatioNResult.IsFailed)
                return validatioNResult;

            var menu = new Menu(id, groups);
            return Result.Ok(menu);
        }
        public List<Group> Groups { get; }
        private Menu(MenuId id, List<Group> groups) : base(id)
        {
            Groups = groups;
            _domainEvents.Add(new MenuCreatedEvent(this, id));
        }

        private static Result CreationValidation(List<Group> groups)
        {
            if (groups is null || groups.Count == 0)
                return Result.Fail("Groups are missing.");

            if (!groups.HasUniqueValues())
                return Result.Fail("Groups has to be unique.");

            return Result.Ok();
        }
    }
}
