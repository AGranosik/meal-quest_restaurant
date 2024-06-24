using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Menus.ValueObjects;

namespace Restaurant.UnitTests.Menus.ValueObejcts
{
    [TestFixture]
    public class MealTests
    {
        private readonly List<Ingredient> _ingredients =
        [
            new Ingredient("ingedient"),
            new Ingredient("ingedient2"),
            new Ingredient("ingedient3")
        ];

        private readonly Price _price = new(20.23m);

        [Test]
        public void Creation_Ingredients_CannotBeNull()
        {
            var creation = () => new Meal(null!, null!);
            creation.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Creation_Ingredients_CannotBeEmpty()
        {
            var creation = () => new Meal(new List<Ingredient>(), null!);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Ingredients_HaveToBeUnique()
        {
            var creation = () => new Meal(
            [
                new("test"),
                new("test2"),
                new("test"),
            ], null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Price_CannotBeNull()
        {
            var creation = () => new Meal(_ingredients, null!);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new Meal(_ingredients, _price);
            creation.Should().NotThrow();
        }
    }
}
