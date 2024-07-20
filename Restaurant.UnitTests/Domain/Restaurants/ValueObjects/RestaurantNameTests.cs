using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects
{
    [TestFixture]
    internal class RestaurantNameTests
    {
        [Test]
        public void CannotBeEmpty_ThrowsException()
        {
            var action = () => new RestaurantName(string.Empty);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeNull_ThrowsException()
        {
            var action = () => new RestaurantName(null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeWhiteSpace_ThrowsException()
        {
            var action = () => new RestaurantName(" ");
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeMultipleWhiteSpace_ThrowsException()
        {
            var action = () => new RestaurantName("    ");
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Equality_DifferentStreetNamesNotEqual_False()
        {
            (new RestaurantName("test") == new RestaurantName("different name")).Should().BeFalse();
        }

        [Test]
        public void Equality_SameName_True()
        {
            var name = "test name";
            (new RestaurantName(name) == new RestaurantName(name))
                .Should().BeTrue();
        }
    }
}
