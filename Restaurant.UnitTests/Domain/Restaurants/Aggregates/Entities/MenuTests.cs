using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.Aggregates.Entities;

[TestFixture]
public class MenuTests
{
    [Test]
    public void Creation_MenuIdCannotBeNull()
    {
        var creationResult = Menu.Create(null!, null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_NameCannotBeNull()
    {
        var creationResult = Menu.Create(new MenuId(10), null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_Success()
    {
        var creationResult = Menu.Create(new MenuId(10), new Name("test"));
        creationResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void Equality_SameReference_True()
    {
        var creationResult = Menu.Create(new MenuId(10), new Name("test"));
        var menu = creationResult.Value;

        (menu == menu).Should().BeTrue();
    }

    [Test]
    public void Equality_DifferentMenuId_False()
    {
        var creationResult = Menu.Create(new MenuId(10), new Name("test"));
        var creationResult2 = Menu.Create(new MenuId(9), new Name("test"));
        var menu = creationResult.Value;
        var menu2 = creationResult2.Value;

        (menu == menu2).Should().BeFalse();
    }

    [Test]
    public void Equality_DifferentNameSameId_True()
    {
        var creationResult = Menu.Create(new MenuId(9), new Name("test"));
        var creationResult2 = Menu.Create(new MenuId(9), new Name("test2"));
        var menu = creationResult.Value;
        var menu2 = creationResult2.Value;

        (menu == menu2).Should().BeTrue();
    }
}