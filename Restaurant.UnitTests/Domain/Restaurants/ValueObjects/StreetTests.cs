using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects
{
    [TestFixture]
    public class StreetTests
    {
        [Test]
        public void CannotBeEmpty_ThrowsException()
        {
            var action = () => new Street(string.Empty);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeNull_ThrowsException()
        {
            var action = () => new Street(null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeWhiteSpace_ThrowsException()
        {
            var action = () => new Street(" ");
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CannotBeMultipleWhiteSpace_ThrowsException()
        {
            var action = () => new Street("    ");
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void DifferentStreetNamesNotEqual_False()
        {
            (new Street("test") == new Street("different name")).Should().BeFalse();
        }

        [Test]
        public void SameName_True()
        {
            var name = "test name";
            (new Street(name) == new Street(name))
                .Should().BeTrue();
        }
    }
}
