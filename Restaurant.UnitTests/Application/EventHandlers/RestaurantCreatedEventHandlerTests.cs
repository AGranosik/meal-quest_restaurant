using application.EventHandlers.Interfaces;
using application.EventHandlers.Restaurants;
using application.Menus.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public class RestaurantCreatedEventHandlerTests
    {
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<IEventInfoStorage<RestaurantEvent>> _eventInfoStorageMock = new Mock<IEventInfoStorage<RestaurantEvent>>();

        [SetUp]
        public void SetUp()
        {
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _eventInfoStorageMock = new Mock<IEventInfoStorage<RestaurantEvent>>();
        }

        [Test]
        public void Creation_MediatorCannotBeNull_ThrowsException()
        {
            var creation = () => new RestaurantCreatedEventHandler(null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_EventStorageCannotBeNull_ThrowsException()
        {
            var creation = () => new RestaurantCreatedEventHandler(_menuRepositoryMock.Object, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task RestaurantCreatedEvent_EventIsNull_ThrowsException()
        {
            var handler = CreateEventHandler();
            var action = () => handler.Handle(null!, CancellationToken.None);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task RestaurantCreatedEvent_Success()
        {
            var id = 1;
            _eventInfoStorageMock.Setup(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(id));

            var handler = CreateEventHandler();
            var action = () => handler.Handle(new RestaurantCreatedEvent(new RestaurantId(1)), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfoStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.Is<bool>(s => s == true), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task RestaurantCreatedEvent_RepoThrowsException_NoException()
        {
            _menuRepositoryMock
                .Setup(m => m.AddRestaurantAsync(It.IsAny<RestaurantIdMenuId>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = CreateEventHandler();
            var action = () => handler.Handle(new RestaurantCreatedEvent(new RestaurantId(1)), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _menuRepositoryMock.Verify(r => r.AddRestaurantAsync(It.IsAny<RestaurantIdMenuId>(), It.IsAny<CancellationToken>()), Times.Exactly(1 + FallbackRetryPoicies.NUMBER_OF_RETRIES));
            _eventInfoStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.Is<bool>(s => s == false), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task RestaurantCreatedEvent_StoreEvent_NoThrowsFurther() // check if logged in feature or 
        {
            var id = 1;
            _eventInfoStorageMock.Setup(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception());

            _menuRepositoryMock
                .Setup(r => r.AddRestaurantAsync(It.IsAny<RestaurantIdMenuId>(), It.IsAny<CancellationToken>()));

            var handler = CreateEventHandler();
            var action = () => handler.Handle(new RestaurantCreatedEvent(new RestaurantId(1)), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _menuRepositoryMock.Verify(r => r.AddRestaurantAsync(It.IsAny<RestaurantIdMenuId>(), It.IsAny<CancellationToken>()), Times.Once());
            _eventInfoStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPoicies.NUMBER_OF_RETRIES + 1));
        }

        private RestaurantCreatedEventHandler CreateEventHandler()
            => new(_menuRepositoryMock.Object, _eventInfoStorageMock.Object);
    }
}
