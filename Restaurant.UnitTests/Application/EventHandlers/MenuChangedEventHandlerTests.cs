using application.EventHandlers;
using application.EventHandlers.Menus;
using application.Restaurants.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using Moq;
using unitTests.DataFakers;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public class MenuChangedEventHandlerTests : AggregateChangedEventHandlerTests<domain.Menus.Aggregates.Menu, domain.Menus.ValueObjects.Identifiers.MenuId>
    {
        private Mock<IRestaurantRepository> _restaurantRepository;

        public override void SetUp()
        {
            _restaurantRepository = new Mock<IRestaurantRepository>();
            base.SetUp();
        }

        [Test]
        public async Task Processing_Success()
        {
            ConfigureRestaurantRepositorySuccessful();
            var @event = CreateValidEvent();
            var handler = CreateHandler();
            var action = () => handler.Handle(@event, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _restaurantRepository.Verify(r => r.AddMenuAsync(It.Is<Menu>(m => m.Id!.Value == MenuDataFaker.MENU_ID), It.Is<RestaurantId>(r => r.Value == MenuDataFaker.RESTAURANT_ID), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Processing_RetryPolicyApplied_Called()
        {
            ConfigureRestaurantRepositoryFailure();
            MockLogger();

            var @event = CreateValidEvent();
            var handler = CreateHandler();
            var action = () => handler.Handle(@event, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _restaurantRepository.Verify(r => r.AddMenuAsync(It.Is<Menu>(m => m.Id!.Value == MenuDataFaker.MENU_ID), It.Is<RestaurantId>(r => r.Value == MenuDataFaker.RESTAURANT_ID), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1));
            CheckIfLoggedError();
        }

        protected override void ConfigureFailureProcessing()
            => ConfigureRestaurantRepositoryFailure();

        protected override void ConfigureSuccessfulProcessing()
            => ConfigureRestaurantRepositorySuccessful();

        protected override AggregateChangedEventHandler<domain.Menus.Aggregates.Menu, domain.Menus.ValueObjects.Identifiers.MenuId> CreateHandler()
            => new MenuChangedEventHandler(_eventInfoStorage.Object, _loggerMock.Object, _restaurantRepository.Object);

        protected override AggregateChangedEvent<domain.Menus.Aggregates.Menu, domain.Menus.ValueObjects.Identifiers.MenuId> CreateValidEvent()
            => new(MenuDataFaker.ValidMenu());

        private void ConfigureRestaurantRepositorySuccessful()
            => _restaurantRepository.Setup(r => r.AddMenuAsync(It.IsAny<domain.Restaurants.Aggregates.Entities.Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        private void ConfigureRestaurantRepositoryFailure()
            => _restaurantRepository.Setup(r => r.AddMenuAsync(It.IsAny<domain.Restaurants.Aggregates.Entities.Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

    }
}
