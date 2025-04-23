using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Menus.ValueObjects;

//TODO: generic tests
//TODO: REMOVE WARNINGS
[TestFixture]
public class CategoryTests
{
    [Test]
    public void CannotBeEmpty_ThrowsException()
    {
        var action = () => new Category(string.Empty);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CannotBeNull_ThrowsException()
    {
        var action = () => new Category(null!);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CannotBeWhiteSpace_ThrowsException()
    {
        var action = () => new Category(" ");
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CannotBeMultipleWhiteSpace_ThrowsException()
    {
        var action = () => new Category("    ");
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void DifferentStreetNamesNotEqual_False()
    {
        (new Category("test") == new Category("different name")).Should().BeFalse();
    }

    [Test]
    public void SameName_True()
    {
        var name = "test name";
        (new Category(name) == new Category(name))
            .Should().BeTrue();
    }
}