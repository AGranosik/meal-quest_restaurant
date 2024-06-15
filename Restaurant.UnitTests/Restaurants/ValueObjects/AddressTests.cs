using FluentAssertions;
using Restaurant.Domain.Restaurants.ValueObjects;

namespace Restaurant.UnitTests.Restaurants.ValueObjects
{
    [TestFixture]
    public class AddressTests
    {
        [Test]
        public void Creation_StreetCannotBeNUll_ThrowsException()
        {
            var action = () => new Address(null, null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CityCannotBeNull_ThrowsException()
        {
            var action = () => new Address(new Street("street"), null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Success()
        {
            var action = () => new Address(new Street("street"), new City("city"));
            action.Should().NotThrow();
        }

        [Test]
        public void Equality_StreetAndCityNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"));
            var address2 = new Address(new Street("street2"), new City("city2"));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_StreetNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"));
            var address2 = new Address(new Street("street2"), new City("city"));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_CityNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"));
            var address2 = new Address(new Street("street"), new City("city2"));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_ValuesTheSame_True()
        {
            var address = new Address(new Street("street"), new City("city"));
            var address2 = new Address(new Street("street"), new City("city"));

            (address == address2).Should().BeTrue();
        }
    }
}
