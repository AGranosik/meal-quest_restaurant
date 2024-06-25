﻿using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Menus.ValueObjects.Identifiers;
using Restaurant.Domain.Restaurants.ValueObjects.Identifiers;

namespace Restaurant.UnitTests.Menus.ValueObejcts.Identifiers
{
    [TestFixture]
    public class MenuIdTests
    {
        [Test]
        public void Creation_RestaurantIdCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuId(null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_NameCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuId(new RestaurantId(2), null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new MenuId(new RestaurantId(2), new Name("test"));
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReferences_True()
        {
            var menuId = new MenuId(new RestaurantId(2), new Name("test"));
            (menuId == menuId).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var menuId = new MenuId(new RestaurantId(2), new Name("test"));
            var menuId2 = new MenuId(new RestaurantId(2), new Name("test"));
            (menuId == menuId2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentRestaurantID_True()
        {
            var menuId = new MenuId(new RestaurantId(2), new Name("test"));
            var menuId2 = new MenuId(new RestaurantId(3), new Name("test"));
            (menuId == menuId2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentNameTrue()
        {
            var menuId = new MenuId(new RestaurantId(2), new Name("test"));
            var menuId2 = new MenuId(new RestaurantId(2), new Name("test2"));
            (menuId == menuId2).Should().BeFalse();
        }
    }
}
