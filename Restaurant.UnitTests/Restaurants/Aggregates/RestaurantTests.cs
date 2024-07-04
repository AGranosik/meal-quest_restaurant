using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Restaurants.Aggregates
{
    [TestFixture]
    public class RestaurantTests
    {
        [Test]
        public void Creation_IdCannotBeNull_ThrowsException()
        {
            var creation = () => Restaurant.Create(null, null, null, null);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_IdOwnerCannotBeNull_Failure()
        {
            var creationResult = Restaurant.Create(new RestaurantId(2), null, null, null);
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_IdOpenningHoursCannotBeNull_Failure()
        {
            //create dummy objects for tests
            var creationResult = Restaurant.Create(new RestaurantId(2), Owner.Create(), null, null);
            creationResult.IsFailed.Should().BeTrue();
        }
    }
}
