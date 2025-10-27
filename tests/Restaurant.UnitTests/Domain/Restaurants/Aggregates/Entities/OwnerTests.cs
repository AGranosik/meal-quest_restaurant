using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.Aggregates.Entities;

[TestFixture]
public class OwnerTests
{
    private readonly Address _validAddress = Address.Create(new Street("street"), new City("city"), new Coordinates(3, 3)).Value;
    private readonly Name _validName = new("name");
    private readonly Name _validSurname = new("surname");

    [Test]
    public void Owner_IstTypeOfEntity_True()
    {
        typeof(Entity<OwnerId>).IsAssignableFrom(typeof(Owner)).Should().BeTrue();
    }

    [Test]
    public void Creation_NameCannotBeNull_Failed()
    {
        var result = Owner.Create(null!, _validName, _validAddress);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_SurnameCannotBeNull_Success()
    {
        var result = Owner.Create(_validName, null!, _validAddress);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_AddressCannotBeNull_Success()
    {
        var result = Owner.Create(_validName, _validSurname, null!);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Equality_SameReference_True()
    {
        var result = Owner.Create(_validName, _validSurname, _validAddress);
        (result.Value == result.Value).Should().BeTrue();
    }
}