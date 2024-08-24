using domain.Common.ValueTypes.Strings;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects.Identifiers
{
    [TestFixture]
    public class MenuRestaurantIdTests
    {
        [Test]
        public void Creation_CanBeNegative_ThrowsException()
        {
            var creation = () => new MenuId(-1);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_CanBeZero_ThrowsException()
        {
            var creation = () => new MenuId(0);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_CanBePositive_ThrowsException()
        {
            var creation = () => new MenuId(1);
            creation.Should().NotThrow();
        }
    }
}
