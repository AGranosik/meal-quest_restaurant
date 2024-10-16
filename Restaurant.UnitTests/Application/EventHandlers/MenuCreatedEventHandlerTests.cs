using application.EventHandlers.Interfaces;
using application.EventHandlers.Menus;
using application.Restaurants.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.DomainEvents;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    internal class MenuCreatedEventHandlerTests
    {
        private Mock<IRestaurantRepository> _restaurantRepositoryMock;
        private Mock<IEventInfoStorage<MenuEvent>> _eventInfoStorageMock;
        private Mock<ILogger<MenuCreatedEventHandler>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            _eventInfoStorageMock = new Mock<IEventInfoStorage<MenuEvent>>();
            _loggerMock = new Mock<ILogger<MenuCreatedEventHandler>>();
        }

        [Test]
        public void Creation_RepositoryCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuCreatedEventHandler(null!, null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_EventStorageCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuCreatedEventHandler(_restaurantRepositoryMock.Object, null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_LoggerCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuCreatedEventHandler(_restaurantRepositoryMock.Object, _eventInfoStorageMock.Object, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new MenuCreatedEventHandler(_restaurantRepositoryMock.Object, _eventInfoStorageMock.Object, _loggerMock.Object);
            creation.Should().NotThrow();
        }

        [Test]
        public async Task Handle_EventIsNull_ThrowsException()
        {
            var handler = CreateHandler();
            var action = () => handler.Handle(null!, CancellationToken.None);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task Handle_Success()
        {
            var id = 1;
            _eventInfoStorageMock.Setup(e => e.StorePendingEvent(It.IsAny<MenuCreatedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(id));

            var handler = CreateHandler();
            var action = () => handler.Handle(CreateEvent(), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfoStorageMock.Verify(e => e.StorePendingEvent(It.IsAny<MenuCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
            _restaurantRepositoryMock.Verify(r => r.AddMenuAsync(It.IsAny<Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Handle_RepoThrowsException_NoException()
        {
            _restaurantRepositoryMock
                .Setup(r => r.AddMenuAsync(It.IsAny<Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = CreateHandler();
            var action = () => handler.Handle(CreateEvent(), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _restaurantRepositoryMock.Verify(r => r.AddMenuAsync(It.IsAny<Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>()), Times.Exactly(1 + FallbackRetryPoicies.NUMBER_OF_RETRIES));
            _eventInfoStorageMock.Verify(e => e.StorePendingEvent(It.IsAny<MenuCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task Handle_StoreEventThrows_NoException()
        {
            _eventInfoStorageMock.Setup(e => e.StorePendingEvent(It.IsAny<MenuCreatedEvent>(),  It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = CreateHandler();
            var action = () => handler.Handle(CreateEvent(), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _restaurantRepositoryMock.Verify(r => r.AddMenuAsync(It.IsAny<Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>()), Times.Once());
            _eventInfoStorageMock.Verify(e => e.StorePendingEvent(It.IsAny<MenuCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPoicies.NUMBER_OF_RETRIES + 1));
        }

        private MenuCreatedEventHandler CreateHandler()
            => new(_restaurantRepositoryMock.Object, _eventInfoStorageMock.Object, _loggerMock.Object);

        private static MenuCreatedEvent CreateEvent()
            => new(new domain.Menus.ValueObjects.Identifiers.MenuId(1), new Name("test"), new RestaurantIdMenuId(2));
    }
}
