using domain.Common.ValueTypes.Strings;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects.Identifiers
{
    [TestFixture]
    public class MenuRestaurantIdTests
    {
        [Test]
        public void Creation_RestaurantIdCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuRestaurantId(null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_NameCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuRestaurantId(new RestaurantId(2), null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new MenuRestaurantId(new RestaurantId(2), new Name("test"));
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReferences_True()
        {
            var menuId = new MenuRestaurantId(new RestaurantId(2), new Name("test"));
            (menuId == menuId).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var menuId = new MenuRestaurantId(new RestaurantId(2), new Name("test"));
            var menuId2 = new MenuRestaurantId(new RestaurantId(2), new Name("test"));
            (menuId == menuId2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentRestaurantID_True()
        {
            var menuId = new MenuRestaurantId(new RestaurantId(2), new Name("test"));
            var menuId2 = new MenuRestaurantId(new RestaurantId(3), new Name("test"));
            (menuId == menuId2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentNameTrue()
        {
            var menuId = new MenuRestaurantId(new RestaurantId(2), new Name("test"));
            var menuId2 = new MenuRestaurantId(new RestaurantId(2), new Name("test2"));
            (menuId == menuId2).Should().BeFalse();
        }
    }
}
