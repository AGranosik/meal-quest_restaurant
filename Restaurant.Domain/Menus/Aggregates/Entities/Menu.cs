using FluentResults;
using core.Extensions;
using domain.Common.BaseTypes;
using domain.Menus.ValueObjects.Identifiers;
using domain.Menus.ValueObjects;
using domain.Menus.Aggregates.DomainEvents;
using core.SimpleTypes;
using domain.Common.ValueTypes.Strings;

namespace domain.Menus.Aggregates.Entities
{
    public sealed class Menu : Entity<MenuId>
    {
        public static Result<Menu> Create(List<Group> groups, Name name)
        {
            var validatioNResult = CreationValidation(groups);
            if (validatioNResult.IsFailed)
                return validatioNResult;

            var menu = new Menu(groups, name);
            return Result.Ok(menu);
        }
        public Name Name { get; }
        public List<Group> Groups { get; }
        private Menu(List<Group> groups, Name name) : base()
        {
            Groups = groups;
            Name = name;
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
