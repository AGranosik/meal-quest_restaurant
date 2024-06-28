using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Menus.Aggregates.Entities;
using Restaurant.Domain.Menus.ValueObjects;
using Restaurant.Domain.Menus.ValueObjects.Identifiers;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.UnitTests.Menus.Entities
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
            _validId = new MenuId(new RestaurantId(2), _validName);
            _validGroups =
            [
                new Group(
                [
                    new(
                    [
                        new Ingredient("test")
                    ], new Price(20), new Name("test"))
                ], new Name("hehe")),
                new Group(
                [
                    new(
                    [
                        new("test2")
                    ], new Price(20), new Name("test2"))
                ], new Name("hehe2"))
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
            var menu2 = Menu.Create(new MenuId(new RestaurantId(_validId.RestaurantId.Id + 1), _validName), _validGroups).Value;
            (menu == menu2).Should().BeFalse();
        }
    }
}
