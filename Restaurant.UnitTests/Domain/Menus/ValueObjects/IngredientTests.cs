using domain.Menus.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Menus.ValueObejcts;

[TestFixture]
public class IngredientTests
{
    private readonly string _validIngredientName = "valid";

    [Test]
    public void Creation_CannotBeNull_ThrowsException()
    {
        var creation = () => Ingredient.Create(null!);
        creation.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Creation_CannotBeEmpty_ThrowsException()
    {
        var creation = () => Ingredient.Create(string.Empty);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_CannotBeWhiteSpaces_ThrowsException()
    {
        var creation = () => Ingredient.Create("         ");
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_Success()
    {
        var creation = () => Ingredient.Create(_validIngredientName);
        creation.Should().NotThrow();
    }

    [Test]
    public void Equality_SameReference_True()
    {
        var ingredient = Ingredient.Create(_validIngredientName);
        (ingredient.Value == ingredient.Value).Should().BeTrue();
    }

    [Test]
    public void Equality_SameValue_True()
    {
        var ingredient = Ingredient.Create(_validIngredientName);
        var ingredient2 = Ingredient.Create(_validIngredientName);
        (ingredient.Value == ingredient2.Value).Should().BeTrue();
    }

    [Test]
    public void Equality_DifferentValue_False()
    {
        var creationResult = Ingredient.Create(_validIngredientName);
        var creationResult2 = Ingredient.Create(_validIngredientName + "hasd");
        (creationResult.Value == creationResult2.Value).Should().BeFalse();
    }
}