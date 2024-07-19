using FluentResults;
using core.Extensions;
using domain.Common.BaseTypes;
using domain.Menus.ValueObjects.Identifiers;
using domain.Menus.ValueObjects;
using domain.Menus.Aggregates.DomainEvents;

namespace domain.Menus.Aggregates.Entities
{
    // Test containers
    public sealed class Menu : Entity<MenuId>
    {
        // remove Id from constructors or it should be able to assign nulls? Before db creation
        public static Result<Menu> Create(List<Group> groups)
        {
            var validatioNResult = CreationValidation(groups);
            if (validatioNResult.IsFailed)
                return validatioNResult;

            var menu = new Menu(groups);
            return Result.Ok(menu);
        }
        public List<Group> Groups { get; }
        private Menu(List<Group> groups) : base()
        {
            Groups = groups;
            _domainEvents.Add(new MenuCreatedEvent(this));
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
