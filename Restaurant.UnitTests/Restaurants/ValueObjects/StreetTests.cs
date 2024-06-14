using System.IO;
using FluentAssertions;
using Restaurant.Domain.Restaurants.ValueObjects;

namespace Restaurant.UnitTests.Restaurants.ValueObjects
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
    }
}
