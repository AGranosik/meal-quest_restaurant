
using domain.Menus.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Menus.ValueObejcts.Identifiers;

[TestFixture]
public class MenuIdTests
{
    [Test]
    public void Creation_CanBe0_Success()
    {
        var creation = () => new MenuId(0);
        creation.Should().NotThrow();
    }

    [Test]
    public void Creation_CanBePoistive_Success()
    {
        var creation = () => new MenuId(1);
        creation.Should().NotThrow();
    }

    [Test]
    public void Creation_CanBeNegative_Success()
    {
        var creation = () => new MenuId(1);
        creation.Should().NotThrow();
    }
}