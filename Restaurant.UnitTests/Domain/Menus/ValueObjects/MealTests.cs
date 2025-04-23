using domain.Common.ValueTypes.Numeric;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Menus.ValueObjects;

[TestFixture]
public class MealTests
{
    private List<Ingredient> _ingredients;

    private Price _price;

    private Name _name;
    private List<Category> _categories;

    [SetUp]
    public void SetUp()
    {
        _price = new(20.23m);
        _ingredients =
        [
            Ingredient.Create("ingedient").Value,
            Ingredient.Create("ingedient2").Value,
            Ingredient.Create("ingedient3").Value,
        ];
        _name = new Name("name");
        _categories =
        [
            new Category("cat3"),
            new Category("cat2")
        ];
    }

    [Test]
    public void Creation_Ingredients_CannotBeNull()
    {
        var creation = () => new Meal(null!, null!, null!, null!);
        creation.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Creation_Ingredients_CannotBeEmpty()
    {
        var creation = () => new Meal(new List<Ingredient>(),  null!, null!, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_Ingredients_HaveToBeUnique()
    {
        var creation = () => new Meal(
        [
            Ingredient.Create("test").Value,
            Ingredient.Create("test2").Value,
            Ingredient.Create("test").Value,
        ], null!,null!, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_Categories_CannotBeNull()
    {
        var creation = () => new Meal(
        _ingredients, null!,null!, null!);
        creation.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void Creation_Categories_CannotBeEmpty()
    {
        var creation = () => new Meal(_ingredients, [],null!, null!);
        creation.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void Creation_Categories_HaveToBeUnique()
    {
        var creation = () => new Meal(_ingredients, 
            [new Category("heh"), new Category("heh2"), new Category("heh")],
            null!, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_Price_CannotBeNull()
    {
        var creation = () => new Meal(_ingredients, null!,null!, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_Name_CannotBeNull()
    {
        var creation = () => new Meal(_ingredients, _categories, _price,null!);
        creation.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Creation_Success()
    {
        var creation = () => new Meal(_ingredients,_categories, _price, _name);
        creation.Should().NotThrow();
    }

    [Test]
    public void Equality_SameReferences_True()
    {
        var meal = new Meal(_ingredients, _categories, _price, _name);
        (meal == meal).Should().BeTrue();
    }

    [Test]
    public void Equality_SameRefernceValues_True()
    {
        var meal = new Meal(_ingredients, _categories, _price, _name);
        var meal2 = new Meal(_ingredients, _categories, _price, _name);
        (meal == meal2).Should().BeTrue();
    }

    [Test]
    public void Equality_SameValues_True()
    {
        List<Ingredient> ingredients =
        [
            Ingredient.Create("ingedient").Value,
            Ingredient.Create("ingedient2").Value,
            Ingredient.Create("ingedient3").Value
        ];
        Price price = new(20.23m);
        Name name = new("name");
        var meal = new Meal(_ingredients, _categories, price, _name);
        var meal2 = new Meal(ingredients, _categories, _price, name);
        (meal == meal2).Should().BeTrue();
    }

    [Test]
    public void Equality_DiffrentNumberOfIngredients_False()
    {
        List<Ingredient> ingredients =
        [
            Ingredient.Create("ingedient").Value,
            Ingredient.Create("ingedient2").Value,
            Ingredient.Create("ingedient3").Value,
            Ingredient.Create("ingedient4").Value
        ];

        var meal = new Meal(_ingredients, _categories, _price, _name);
        var meal2 = new Meal(ingredients, _categories, _price, _name);
        (meal == meal2).Should().BeFalse();
    }

    [Test]
    public void Equality_DiffrentIngredient_False()
    {
        List<Ingredient> ingredients =
        [
            Ingredient.Create("ingedient").Value,
            Ingredient.Create("ingedient2").Value,
            Ingredient.Create("ingedient3").Value,
            Ingredient.Create("ingedient4").Value
        ];
        List<Ingredient> ingredients2 =
        [
            Ingredient.Create("ingedient").Value,
            Ingredient.Create("ingedient2").Value,
            Ingredient.Create("ingedient3").Value,
            Ingredient.Create("ingedient0").Value
        ];
        var meal = new Meal(ingredients, _categories, _price, _name);
        var meal2 = new Meal(ingredients2, _categories, _price, _name);
        (meal == meal2).Should().BeFalse();
    }

    [Test]
    public void Equality_DifferentPrice_False()
    {
        var meal = new Meal(_ingredients, _categories, _price, _name);
        var meal2 = new Meal(_ingredients, _categories, new Price(0.33m), _name);
        (meal == meal2).Should().BeFalse();
    }

    [Test]
    public void Equality_DifferentName_False()
    {
        var meal = new Meal(_ingredients, _categories, _price, _name);
        var meal2 = new Meal(_ingredients, _categories, _price, new Name("hehe"));
        (meal == meal2).Should().BeFalse();
    }
}