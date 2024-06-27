using FluentResults;
using Restaurant.Core.Extensions;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Menus.ValueObjects;
using Restaurant.Domain.Menus.ValueObjects.Identifiers;

namespace Restaurant.Domain.Menus.Entities
{
    //available hours
    public class Menu : Entity<MenuId>
    {
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
        }

        private static Result CreationValidation(List<Group> groups)
        {
            if (groups is null || groups.Count == 0)
                return Result.Fail("Brak grup.");

            if (!groups.HasUniqueValues())
                return Result.Fail("Grupy musza byc unikalne.");

            return Result.Ok();
        }
    }
}
