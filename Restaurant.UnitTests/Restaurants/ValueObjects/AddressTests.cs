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
            var action = () => new Address(null, null, null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CityCannotBeNull_ThrowsException()
        {
            var action = () => new Address(new Street("street"), null, null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CoordinatesCannotBeNull_ThrowsException()
        {
            var action = () => new Address(new Street("street"), new City("city"), null);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Success()
        {
            var action = () => new Address(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
            action.Should().NotThrow();
        }

        [Test]
        public void Equality_AnyValuesNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
            var address2 = new Address(new Street("street2"), new City("city2"), new Coordinates(51.86424635360838, 19.378350696319068));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_StreetNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"), new Coordinates(51.864246353260848, 19.3783506296319078));
            var address2 = new Address(new Street("street2"), new City("city"), new Coordinates(51.864246352360848, 19.3783502696319078));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_CityNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
            var address2 = new Address(new Street("street"), new City("city2"), new Coordinates(51.86424635360847, 19.378350696319077));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_CooridinatesNotTheSame_False()
        {
            var address = new Address(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
            var address2 = new Address(new Street("street"), new City("city2"), new Coordinates(51.86424635360846, 19.378350696319077));

            (address == address2).Should().BeFalse();
        }

        [Test]
        public void Equality_ValuesTheSame_True()
        {
            var address = new Address(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
            var address2 = new Address(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));

            (address == address2).Should().BeTrue();
        }
    }
}
