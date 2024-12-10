using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using sharedTests.DataFakers;

namespace integrationTests.EventsStorage
{
    [TestFixture]
    internal class RestaurantChangedEventsTests : BaseEventInfoStorageTests<Restaurant, RestaurantId>
    {
        protected override Restaurant CreateAggregate()
            => RestaurantDataFaker.ValidRestaurant();
    }
}
