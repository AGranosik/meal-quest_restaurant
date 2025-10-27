using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects;

public sealed class DescriptionTests
{
    [Test]
    public void CannotBeEmpty_ThrowsException()
    {
        var action = () => new Description(string.Empty);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CannotBeNull_ThrowsException()
    {
        var action = () => new Description(null!);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CannotBeWhiteSpace_ThrowsException()
    {
        var action = () => new Description(" ");
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CannotBeMultipleWhiteSpace_ThrowsException()
    {
        var action = () => new Description("    ");
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Equality_DifferentStreetNamesNotEqual_False()
    {
        (new Description("test") == new Description("different name")).Should().BeFalse();
    }

    [Test]
    public void Equality_SameName_True()
    {
        var name = "test name";
        (new Description(name) == new Description(name))
            .Should().BeTrue();
    }
}