using application.EventHandlers;
using application.EventHandlers.Restaurants;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using unitTests.DataFakers;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public class RestaurantChangedEventHandlerTests : AggregateChangedEventHandlerTests<Restaurant, RestaurantId>
    {

        protected override AggregateChangedEventHandler<Restaurant, RestaurantId> CreateHandler()
            => new RestaurantChangedEventHandler(_eventInforStorage.Object);

        protected override AggregateChangedEvent<Restaurant, RestaurantId> CreateValidEvent()
            => new(RestaurantDataFaker.ValidRestaurant());
    }
}
