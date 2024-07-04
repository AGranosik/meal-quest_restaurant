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
        private RestaurantId _validRestaurantId;
        private Owner _validOwner;
        private OpeningHours _validOpeningHours;
        public RestaurantTests()
        {
            _validRestaurantId = new RestaurantId(2);
            _validOwner = Owner.Create(new OwnerId(2), new Name("test"), new Name("surname"), new Address(new Street("street"), new City("city"), new Coordinates(10, 10))).Value;
            _validOpeningHours = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 00)).Value;
        }

        [Test]
        public void Creation_IdOwnerCannotBeNull_Failure()
        {
            var creationResult = Restaurant.Create(_validRestaurantId, null, null);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_IdOpenningHoursCannotBeNull_Failure()
        {
            var creationResult = Restaurant.Create(_validRestaurantId, _validOwner, null);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_IdCannotBeNull_ThrowsException()
        {
            var creation = () => Restaurant.Create(null, _validOwner, _validOpeningHours);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creationResult = Restaurant.Create(_validRestaurantId, _validOwner, _validOpeningHours);
            creationResult.IsSuccess.Should().BeTrue();
        }
    }
}
