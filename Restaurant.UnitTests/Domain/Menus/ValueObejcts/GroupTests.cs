using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Menus.ValueObejcts
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
            _validIngredients =
            [
                Ingredient.Create("test").Value,
                Ingredient.Create("test2").Value,
                Ingredient.Create("test3").Value,
            ];
            _validMeals = [
                new Meal(_validIngredients.Take(1).ToList(), new Price(20), new Name("test")),
                new Meal(_validIngredients.Skip(1).Take(1).ToList(), new Price(20), new Name("test")),
                new Meal(_validIngredients.Skip(2).Take(1).ToList(), new Price(20), new Name("test")),
                ];

            _validName = new Name("test");
        }

        [Test]
        public void Creation_MealsCannotBeNull_Fail()
        {
            var creationResult = Group.Create(null!, null!);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_MealsCannotBeEmpty_Fail()
        {
            var creationResult = Group.Create([], null!);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_MealsHaveToBeUnique_Fail()
        {
            var creationResult = Group.Create(
            [
                _validMeals[0],
                _validMeals[1],
                _validMeals[2],
                _validMeals[0],
            ], null);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_NameCannotBeNull_Fail()
        {
            var creationResult = Group.Create(_validMeals, null!);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_Success()
        {
            var creationResult = Group.Create(_validMeals, _validName);
            creationResult.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var creationResult = Group.Create(_validMeals, _validName);
            (creationResult.Value == creationResult.Value).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var group = Group.Create(_validMeals, _validName).Value;
            var group2 = Group.Create(_validMeals, _validName).Value;
            (group == group2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentMeals_False()
        {
            var group = Group.Create(_validMeals, _validName).Value;
            var group2 = Group.Create(_validMeals.Take(2).ToList(), _validName).Value;
            (group == group2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentName_False()
        {
            var group = Group.Create(_validMeals, _validName).Value;
            var group2 = Group.Create(_validMeals, new Name("tedfgdfst")).Value;
            (group == group2).Should().BeFalse();
        }
    }
}
