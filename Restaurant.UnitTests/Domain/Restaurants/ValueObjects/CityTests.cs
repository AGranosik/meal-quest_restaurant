using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects
{
    [TestFixture]
    public class CityTests
    {
        [Test]
        public void CannotBeEmpty_ThrowsException()
        {
            var action = () => new City(string.Empty);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeNull_ThrowsException()
        {
            var action = () => new City(null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeWhiteSpace_ThrowsException()
        {
            var action = () => new City(" ");
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeMultipleWhiteSpace_ThrowsException()
        {
            var action = () => new City("    ");
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Equality_DifferentStreetNamesNotEqual_False()
        {
            (new City("test") == new City("different name")).Should().BeFalse();
        }

        [Test]
        public void Equality_SameName_True()
        {
            var name = "test name";
            (new City(name) == new City(name))
                .Should().BeTrue();
        }
    }
}
