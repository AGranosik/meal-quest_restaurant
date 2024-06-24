using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Menus.ValueObjects;

namespace Restaurant.UnitTests.Menus.ValueObejcts
{
    [TestFixture]
    public class MealTests
    {
        private List<Ingredient> _ingredients;

        private Price _price;

        [SetUp]
        public void SetUp()
        {
            _price = new(20.23m);
            _ingredients =
            [
                new Ingredient("ingedient"),
                new Ingredient("ingedient2"),
                new Ingredient("ingedient3")
            ];
        }

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

        [Test]
        public void Equality_SameReferences_True()
        {
            var meal = new Meal(_ingredients, _price);
            (meal == meal).Should().BeTrue();
        }

        [Test]
        public void Equality_SameRefernceValues_True()
        {
            var meal = new Meal(_ingredients, _price);
            var meal2 = new Meal(_ingredients, _price);
            (meal == meal2).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            List<Ingredient> ingredients =
            [
                new Ingredient("ingedient"),
                new Ingredient("ingedient2"),
                new Ingredient("ingedient3")
            ];
            Price price = new(20.23m);
            var meal = new Meal(_ingredients, price);
            var meal2 = new Meal(ingredients, _price);
            (meal == meal2).Should().BeTrue();
        }

        [Test]
        public void Equality_DiffrentNumberOfIngredients_False()
        {
            List<Ingredient> ingredients =
            [
                new Ingredient("ingedient"),
                new Ingredient("ingedient2"),
                new Ingredient("ingedient3"),
                new Ingredient("ingedient4")
            ];

            var meal = new Meal(_ingredients, _price);
            var meal2 = new Meal(ingredients, _price);
            (meal == meal2).Should().BeFalse();
        }

        [Test]
        public void Equality_DiffrentIngredient_False()
        {
            List<Ingredient> ingredients =
            [
                new Ingredient("ingedient"),
                new Ingredient("ingedient2"),
                new Ingredient("ingedient3"),
                new Ingredient("ingedient4")
            ];
            List<Ingredient> ingredients2 =
            [
                new Ingredient("ingedient"),
                new Ingredient("ingedient2"),
                new Ingredient("ingedient3"),
                new Ingredient("ingedient0")
            ];
            var meal = new Meal(ingredients, _price);
            var meal2 = new Meal(ingredients2, _price);
            (meal == meal2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentPrice_False()
        {
            var meal = new Meal(_ingredients, _price);
            var meal2 = new Meal(_ingredients, new Price(0.33m));
            (meal == meal2).Should().BeFalse();
        }
    }
}
