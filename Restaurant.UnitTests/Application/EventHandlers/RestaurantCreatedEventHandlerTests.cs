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
        private Mock<IEventInfoStorage<RestaurantCreatedEvent>> _eventInfStorageMock = new Mock<IEventInfoStorage<RestaurantCreatedEvent>>();

        [SetUp]
        public void SetUp()
        {
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _eventInfStorageMock = new Mock<IEventInfoStorage<RestaurantCreatedEvent>>();
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
        public async Task RestaurantCreatedEvent_RestaurantMenuIdCannotBeNull_Success()
        {
            var handler = CreateEventHandler();
            var action = () => handler.Handle(new RestaurantCreatedEvent(null!), CancellationToken.None);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task RestaurantCreatedEvent_Success()
        {
            var id = 1;
            _eventInfStorageMock.Setup(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(id));

            var handler = CreateEventHandler();
            var action = () => handler.Handle(new RestaurantCreatedEvent(new RestaurantId(1)), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
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
            _eventInfStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task RestaurantCreatedEvent_StoreEvent_ThrowsException()
        {
            var id = 1;
            _eventInfStorageMock.Setup(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception());

            _menuRepositoryMock
                .Setup(r => r.AddRestaurantAsync(It.IsAny<RestaurantIdMenuId>(), It.IsAny<CancellationToken>()));

            var handler = CreateEventHandler();
            var action = () => handler.Handle(new RestaurantCreatedEvent(new RestaurantId(1)), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _menuRepositoryMock.Verify(r => r.AddRestaurantAsync(It.IsAny<RestaurantIdMenuId>(), It.IsAny<CancellationToken>()), Times.Once());
            _eventInfStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<RestaurantCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPoicies.NUMBER_OF_RETRIES + 1));
        }

        private RestaurantCreatedEventHandler CreateEventHandler()
            => new(_menuRepositoryMock.Object, _eventInfStorageMock.Object);
    }
}
