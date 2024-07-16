using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Menus.Entities
{
    [TestFixture]
    public class MenuTests
    {
        private List<Group> _validGroups;
        private MenuId _validId;
        private Name _validName;

        [SetUp]
        public void SetUp()
        {
            _validName = new Name("test");
            _validId = new MenuId(new RestaurantIdMenuId(2), _validName);
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
        public void Creation_GroupsCannotBeNull_FailureResult()
        {
            var result = Menu.Create(_validId, null);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsCannotBeEmpty_FailureResult()
        {
            var result = Menu.Create(_validId, []);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsHaveToBeUnique_FailureResult()
        {
            var group = _validGroups.First();
            var result = Menu.Create(_validId, [group, group]);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_MenuIdCannotBeNull_ThrowsException()
        {
            var creation = () => Menu.Create(null, _validGroups);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_SuccessResult()
        {
            var menu = Menu.Create(_validId, _validGroups);
            menu.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var menu = Menu.Create(_validId, _validGroups).Value;
            (menu == menu).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var menu = Menu.Create(_validId, _validGroups).Value;
            var menu2 = Menu.Create(_validId, _validGroups).Value;
            (menu == menu2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentMenuId_False()
        {
            var menu = Menu.Create(_validId, _validGroups).Value;
            var menu2 = Menu.Create(new MenuId(new RestaurantIdMenuId(_validId.RestaurantId.Value + 1), _validName), _validGroups).Value;
            (menu == menu2).Should().BeFalse();
        }
    }
}
