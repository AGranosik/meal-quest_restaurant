using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentAssertions;
using sharedTests.DataFakers;

namespace unitTests.Domain.Menus.Entities;

[TestFixture]
public class MenuTests
{
    private List<Group> _validGroups;
    private Name _validName;
    private MenuRestaurant _validRestaurant;
    private List<Category> _validCategories;

    [SetUp]
    public void SetUp()
    {
        _validName = MenuDataFaker.ValidName;
        _validGroups = MenuDataFaker.ValidGroups;
        _validRestaurant = MenuDataFaker.ValidRestaurant;
        _validCategories = MenuDataFaker.ValidCategories;
        
    }

    [Test]
    public void Menu_IstTypeOfEntity_True()
    {
        typeof(Entity<MenuId>).IsAssignableFrom(typeof(Menu)).Should().BeTrue();
    }

    [Test]
    public void Creation_GroupsCannotBeNull_FailureResult()
    {
        var result = Menu.Create(null!,null!, null!);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_GroupsCannotBeEmpty_FailureResult()
    {
        var result = Menu.Create([],null!, null!);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_GroupsHaveToBeUnique_FailureResult()
    {
        var group = _validGroups[0];
        var result = Menu.Create([group, group],null!, null!);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_CategoriesCannotBeNull_FailureResult()
    {
        var result = Menu.Create(_validGroups,null!, null!);
        result.IsFailed.Should().BeTrue();
    }
    
    [Test]
    public void Creation_NameCannotBeNull_FailureResult()
    {
        var result = Menu.Create(_validGroups,null!, null!);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_RestaurantIdCannotBeNull_FailureResult()
    {
        var result = Menu.Create(_validGroups, _validName, null!);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_SuccessResult()
    {
        var menu = Menu.Create(_validGroups, _validName, _validRestaurant);
        menu.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void Equality_SameReference_True()
    {
        var menu = Menu.Create(_validGroups, _validName, _validRestaurant).Value;
        (menu == menu).Should().BeTrue();
    }

}