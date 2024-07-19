using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Restaurants.Aggregates
{
    [TestFixture]
    public class RestaurantTests
    {
        private Owner _validOwner;
        private OpeningHours _validOpeningHours;
        public RestaurantTests()
        {
            _validOwner = Owner.Create(new Name("test"), new Name("surname"), Address.Create(new Street("street"), new City("city"), new Coordinates(10, 10)).Value).Value;
            _validOpeningHours = OpeningHours.Create(new List<WorkingDay>
            {
                WorkingDay.Create(DayOfWeek.Monday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.FreeDay(DayOfWeek.Tuesday).Value,
                WorkingDay.Create(DayOfWeek.Wednesday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Thursday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Saturday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Sunday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
            }).Value;
        }

        [Test]
        public void Restaurant_IstTypeOfEntity_True()
        {
            typeof(Entity<RestaurantId>).IsAssignableFrom(typeof(Restaurant)).Should().BeTrue();
        }

        [Test]
        public void Creation_IdOwnerCannotBeNull_Failure()
        {
            var creationResult = Restaurant.Create(null, null);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_IdOpenningHoursCannotBeNull_Failure()
        {
            var creationResult = Restaurant.Create(_validOwner, null);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_Success()
        {
            var creationResult = Restaurant.Create(_validOwner, _validOpeningHours);
            creationResult.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void AddMenu_None_Success()
        {
            var menu = new MenuRestaurantId(new RestaurantId(3), new Name("test"));
            var creationResult = Restaurant.Create(_validOwner, _validOpeningHours);
            var restaurant = creationResult.Value;

            var addResult = restaurant.AddMenu(menu);
            addResult.IsSuccess.Should().BeTrue();

            restaurant.Menus.Count.Should().Be(1);
            var storedMenu = restaurant.Menus.First();
            
            (storedMenu == menu).Should().BeTrue();
        }

        [Test]
        public void AddMenu_OneAlreadyThere_Success()
        {
            var menu = new MenuRestaurantId(new RestaurantId(3), new Name("test"));
            var menu2 = new MenuRestaurantId(new RestaurantId(3), new Name("test2"));
            var creationResult = Restaurant.Create(_validOwner, _validOpeningHours);
            var restaurant = creationResult.Value;
            restaurant.AddMenu(menu);

            var addResult = restaurant.AddMenu(menu2);
            addResult.IsSuccess.Should().BeTrue();

            restaurant.Menus.Count.Should().Be(2);
            restaurant.Menus.Any(m => m == menu2)
                .Should().BeTrue();
        }

        [Test]
        public void AddMenu_CannotAddDuplicates_Failure()
        {
            var menu = new MenuRestaurantId(new RestaurantId(3), new Name("test"));
            var menu2 = new MenuRestaurantId(new RestaurantId(3), new Name("test2"));
            var creationResult = Restaurant.Create(_validOwner, _validOpeningHours);
            var restaurant = creationResult.Value;
            restaurant.AddMenu(menu);
            restaurant.AddMenu(menu2);

            var addResult = restaurant.AddMenu(menu2);
            addResult.IsFailed.Should().BeTrue();
        }
    }
}
