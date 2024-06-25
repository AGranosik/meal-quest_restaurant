using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Menus.ValueObjects;

namespace Restaurant.UnitTests.Menus.ValueObejcts
{
    [TestFixture]
    public class MealTests
    {
        private List<Ingredient> _ingredients;

        private Price _price;

        private Name _name;

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
            _name = new Name("name");
        }

        [Test]
        public void Creation_Ingredients_CannotBeNull()
        {
            var creation = () => new Meal(null!, null!, null!);
            creation.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Creation_Ingredients_CannotBeEmpty()
        {
            var creation = () => new Meal(new List<Ingredient>(), null!, null!);
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
            ], null, null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Price_CannotBeNull()
        {
            var creation = () => new Meal(_ingredients, null!, null!);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Name_CannotBeNull()
        {
            var creation = () => new Meal(_ingredients, _price, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new Meal(_ingredients, _price, _name);
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReferences_True()
        {
            var meal = new Meal(_ingredients, _price, _name);
            (meal == meal).Should().BeTrue();
        }

        [Test]
        public void Equality_SameRefernceValues_True()
        {
            var meal = new Meal(_ingredients, _price, _name);
            var meal2 = new Meal(_ingredients, _price, _name);
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
            Name name = new("name");
            var meal = new Meal(_ingredients, price, _name);
            var meal2 = new Meal(ingredients, _price, name);
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

            var meal = new Meal(_ingredients, _price, _name);
            var meal2 = new Meal(ingredients, _price, _name);
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
            var meal = new Meal(ingredients, _price, _name);
            var meal2 = new Meal(ingredients2, _price, _name);
            (meal == meal2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentPrice_False()
        {
            var meal = new Meal(_ingredients, _price, _name);
            var meal2 = new Meal(_ingredients, new Price(0.33m), _name);
            (meal == meal2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentName_False()
        {
            var meal = new Meal(_ingredients, _price, _name);
            var meal2 = new Meal(_ingredients, _price, new Name("hehe"));
            (meal == meal2).Should().BeFalse();
        }
    }
}
