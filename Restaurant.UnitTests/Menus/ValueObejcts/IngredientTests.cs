using FluentAssertions;
using Restaurant.Domain.Menus.ValueObjects;

namespace Restaurant.UnitTests.Menus.ValueObejcts
{
    [TestFixture]
    public class IngredientTests
    {
        private readonly string _validIngredientName = "valid";

        [Test]
        public void Creation_CannotBeNull_ThrowsException()
        {
            var creation = () => new Ingredient(null);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_CannotBeEmpty_ThrowsException()
        {
            var creation = () => new Ingredient(string.Empty);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CannotBeWhiteSpaces_ThrowsException()
        {
            var creation = () => new Ingredient("         ");
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new Ingredient(_validIngredientName);
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var ingredient = new Ingredient(_validIngredientName);
            (ingredient == ingredient).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValue_True()
        {
            var ingredient = new Ingredient(_validIngredientName);
            var ingredient2 = new Ingredient(_validIngredientName);
            (ingredient == ingredient2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentValue_False()
        {
            var ingredient = new Ingredient(_validIngredientName);
            var ingredient2 = new Ingredient(_validIngredientName + "hasd");
            (ingredient == ingredient2).Should().BeFalse();
        }
    }
}
