using FluentAssertions;
using Restaurant.Domain.Common.ValueTypes.Numeric;
using Restaurant.Domain.Common.ValueTypes.Strings;
using Restaurant.Domain.Menus.ValueObjects;

namespace Restaurant.UnitTests.Menus.ValueObejcts
{
    [TestFixture]
    public class GroupTests
    {
        private List<Meal> _validMeals;
        private List<Ingredient> _validIngredients;
        private Name _validName;

        [SetUp]
        public void SetUp()
        {
            _validIngredients = new List<Ingredient>
            {
                new("test"),
                new("test2"),
                new("test3"),
            };
            _validMeals = [
                new Meal(_validIngredients.Take(1).ToList(), new Price(20), new Name("test")),
                new Meal(_validIngredients.Skip(1).Take(1).ToList(), new Price(20), new Name("test")),
                new Meal(_validIngredients.Skip(2).Take(1).ToList(), new Price(20), new Name("test")),
                ];

            _validName = new Name("test");
        }

        [Test]
        public void Creation_MealsCannotBeNull_ThrowsException()
        {
            var creation = () => new Group(null, null);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_MealsCannotBeEmpty_ThrowsException()
        {
            var creation = () => new Group(new List<Meal>(), null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_MealsHaveToBeUnique_ThrowsException()
        {
            var creation = () => new Group(new List<Meal>
            {
                _validMeals[0],
                _validMeals[1],
                _validMeals[2],
                _validMeals[0],
            }, null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_NameCannotBeNull_ThrowsException()
        {
            var creation = () => new Group(_validMeals, null);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new Group(_validMeals, _validName);
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var group = new Group(_validMeals, _validName);
            (group == group).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var group = new Group(_validMeals, _validName);
            var group2 = new Group(_validMeals, _validName);
            (group == group2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentMeals_False()
        {
            var group = new Group(_validMeals, _validName);
            var group2 = new Group(_validMeals.Take(2).ToList(), _validName);
            (group == group2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentName_False()
        {
            var group = new Group(_validMeals, _validName);
            var group2 = new Group(_validMeals, new Name("tedfgdfst"));
            (group == group2).Should().BeFalse();
        }
    }
}
