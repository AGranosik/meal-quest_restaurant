using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects;

[TestFixture]
public class AddressTests
{
    [Test]
    public void Creation_StreetCannotBeNUll_FailedResult()
    {
        var creationResult = Address.Create(null!, null!, null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_CityCannotBeNull_FailedResult()
    {
        var creationResult = Address.Create(new Street("street"), null!, null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_CoordinatesCannotBeNull_FailedResult()
    {
        var creationResult = Address.Create(new Street("street"), new City("city"), null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_Success()
    {
        var creationResult = Address.Create(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
        creationResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void Equality_AnyValuesNotTheSame_False()
    {
        var addressCreationResult = Address.Create(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
        var addressCreationResult2 = Address.Create(new Street("street2"), new City("city2"), new Coordinates(51.86424635360838, 19.378350696319068));

        (addressCreationResult.Value == addressCreationResult2.Value).Should().BeFalse();
    }

    [Test]
    public void Equality_StreetNotTheSame_False()
    {
        var addressCreationResult = Address.Create(new Street("street"), new City("city"), new Coordinates(51.864246353260848, 19.3783506296319078));
        var addressCreationResult2 = Address.Create(new Street("street2"), new City("city"), new Coordinates(51.864246352360848, 19.3783502696319078));

        (addressCreationResult.Value == addressCreationResult2.Value).Should().BeFalse();
    }

    [Test]
    public void Equality_CityNotTheSame_False()
    {
        var addressCreationResult = Address.Create(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
        var addressCreationResult2 = Address.Create(new Street("street"), new City("city2"), new Coordinates(51.86424635360847, 19.378350696319077));

        (addressCreationResult.Value == addressCreationResult2.Value).Should().BeFalse();
    }

    [Test]
    public void Equality_CooridinatesNotTheSame_False()
    {
        var addressCreationResult = Address.Create(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
        var addressCreationResult2 = Address.Create(new Street("street"), new City("city2"), new Coordinates(51.86424635360846, 19.378350696319077));

        (addressCreationResult.Value == addressCreationResult2.Value).Should().BeFalse();
    }

    [Test]
    public void Equality_ValuesTheSame_True()
    {
        var addressCreationResult = Address.Create(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));
        var addressCreationResult2 = Address.Create(new Street("street"), new City("city"), new Coordinates(51.86424635360848, 19.378350696319078));

        (addressCreationResult.Value == addressCreationResult2.Value).Should().BeTrue();
    }
}