using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using sharedTests.DataFakers;

namespace unitTests.Domain.Restaurants.Aggregates;

[TestFixture]
public class RestaurantTests
{
    private readonly Name _validName;
    private readonly Owner _validOwner;
    private readonly OpeningHours _validOpeningHours;
    private readonly Address _validAddress;
    public RestaurantTests()
    {
        _validName = RestaurantDataFaker.ValidRestaurantName;
        _validOwner = RestaurantDataFaker.ValidOwner;
        _validOpeningHours = RestaurantDataFaker.ValidOpeningHours;
        _validAddress = RestaurantDataFaker.ValidRestaurantAddress;
    }

    [Test]
    public void Restaurant_IstTypeOfEntity_True()
    {
        typeof(Aggregate<RestaurantId>).IsAssignableFrom(typeof(Restaurant)).Should().BeTrue();
    }

    [Test]
    public void Creation_NameCannotBeNull_Failure()
    {
        var creationResult = Restaurant.Create(null!, null!, null!, null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_IdOwnerCannotBeNull_Failure()
    {
        var creationResult = Restaurant.Create(_validName, null!, null!, null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_IdOpenningHoursCannotBeNull_Failure()
    {
        var creationResult = Restaurant.Create(_validName, _validOwner, null!, null!);
        creationResult.IsFailed.Should().BeTrue();
    }


    [Test]
    public void Creation_AddressCannotBeNull_Failure()
    {
        var creationResult = Restaurant.Create(_validName, _validOwner, _validOpeningHours, null!);
        creationResult.IsFailed.Should().BeTrue();
    }

    [Test]
    public void Creation_Success()
    {
        var creationResult = Restaurant.Create(_validName, _validOwner, _validOpeningHours, _validAddress);
        creationResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void AddMenu_None_Success()
    {
        var menuResult = Menu.Create(new MenuId(1), new Name("test"));
        var creationResult = Restaurant.Create(_validName, _validOwner, _validOpeningHours, _validAddress);
        var restaurant = creationResult.Value;

        var addResult = restaurant.AddMenu(menuResult.Value);
        addResult.IsSuccess.Should().BeTrue();

        restaurant.Menus.Count.Should().Be(1);
        var storedMenu = restaurant.Menus.First();

        (storedMenu == menuResult.Value).Should().BeTrue();
    }

    [Test]
    public void AddMenu_OneAlreadyThere_Success()
    {
        var menuResult = Menu.Create(new MenuId(1), new Name("test"));
        var menuResult2 = Menu.Create(new MenuId(2), new Name("test2"));
        var creationResult = Restaurant.Create(_validName, _validOwner, _validOpeningHours, _validAddress);
        var restaurant = creationResult.Value;
        restaurant.AddMenu(menuResult.Value);

        var addResult = restaurant.AddMenu(menuResult2.Value);
        addResult.IsSuccess.Should().BeTrue();

        restaurant.Menus.Count.Should().Be(2);
        restaurant.Menus.Any(m => m == menuResult2.Value)
            .Should().BeTrue();
    }

    [Test]
    public void AddMenu_CannotAddDuplicates_Failure()
    {
        var menuResult = Menu.Create(new MenuId(1), new Name("test"));
        var menuResult2 = Menu.Create(new MenuId(2), new Name("test2"));
        var creationResult = Restaurant.Create(_validName, _validOwner, _validOpeningHours, _validAddress);
        var restaurant = creationResult.Value;
        restaurant.AddMenu(menuResult.Value);
        restaurant.AddMenu(menuResult2.Value);

        var addResult = restaurant.AddMenu(menuResult2.Value);
        addResult.IsFailed.Should().BeTrue();
    }
}