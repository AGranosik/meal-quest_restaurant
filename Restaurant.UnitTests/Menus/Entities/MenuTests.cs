using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Menus.Entities;
using Restaurant.Domain.Menus.ValueObjects;
using Restaurant.Domain.Menus.ValueObjects.Identifiers;

namespace Restaurant.UnitTests.Menus.Entities
{
    [TestFixture]
    public class MenuTests
    {
        private Name _validName;
        private List<Group> _validGroups;
        private MenuId _validId;

        [SetUp]
        public void SetUp()
        {
            _validId = new MenuId(2);
            _validName = new Name("test");
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
        public void Creation_NameCannotBeNull_FailureResult()
        {
            var result = Menu.Create(null, null, null);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsCannotBeNull_FailureResult()
        {
            var result = Menu.Create(null, new Name("test"), null);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsCannotBeEmpty_FailureResult()
        {
            var result = Menu.Create(null, new Name("test"), []);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_GroupsHaveToBeUnique_FailureResult()
        {
            var group = _validGroups.First();
            var result = Menu.Create(null, new Name("test"), [group, group]);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_MenuIdCannotBeNull_ThrowsException()
        {
            var creation = () => Menu.Create(null, _validName, _validGroups);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_SuccessResult()
        {
            var menu = Menu.Create(_validId, _validName, _validGroups);
            menu.IsSuccess.Should().BeTrue();
        }
    }
}
