using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Menus.Entities
{
    [TestFixture]
    public class MenuTests
    {
        private List<Group> _validGroups;
        private Name _validName;

        [SetUp]
        public void SetUp()
        {
            _validName = new Name("test");
            _validGroups =
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
        }

        [Test]
        public void Menu_IstTypeOfEntity_True()
        {
            typeof(Entity<MenuId>).IsAssignableFrom(typeof(Menu)).Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsCannotBeNull_FailureResult()
        {
            var result = Menu.Create(null);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsCannotBeEmpty_FailureResult()
        {
            var result = Menu.Create([]);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsHaveToBeUnique_FailureResult()
        {
            var group = _validGroups.First();
            var result = Menu.Create([group, group]);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_SuccessResult()
        {
            var menu = Menu.Create(_validGroups);
            menu.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var menu = Menu.Create(_validGroups).Value;
            (menu == menu).Should().BeTrue();
        }

    }
}
