﻿using application.EventHandlers.Interfaces;
using application.EventHandlers.Menus;
using application.Restaurants.Commands.Interfaces;
using core.FallbackPolicies;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.DomainEvents;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates.DomainEvents;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    internal class MenuCreatedEventHandlerTests
    {
        private Mock<IRestaurantRepository> _restaurantRepositoryMock;
        private Mock<IEventInfoStorage<MenuEvent>> _eventInfoStorageMock;

        [SetUp]
        public void SetUp()
        {
            _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            _eventInfoStorageMock = new Mock<IEventInfoStorage<MenuEvent>>();
        }

        [Test]
        public void Creation_RepositoryCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuCreatedEventHandler(null, null);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_EventStorageCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuCreatedEventHandler(_restaurantRepositoryMock.Object, null);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = () => new MenuCreatedEventHandler(_restaurantRepositoryMock.Object, _eventInfoStorageMock.Object);
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
            _eventInfoStorageMock.Setup(e => e.StoreEventAsync(It.IsAny<MenuCreatedEvent>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(id));

            var handler = CreateHandler();
            var action = () => handler.Handle(CreateEvent(), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfoStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<MenuCreatedEvent>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once());
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
            _eventInfoStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<MenuCreatedEvent>(), It.Is<bool>(s => s == false), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task Handle_StoreEventThrows_NotException()
        {
            _eventInfoStorageMock.Setup(e => e.StoreEventAsync(It.IsAny<MenuCreatedEvent>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = CreateHandler();
            var action = () => handler.Handle(CreateEvent(), CancellationToken.None);
            await action.Should().NotThrowAsync();

            _restaurantRepositoryMock.Verify(r => r.AddMenuAsync(It.IsAny<Menu>(), It.IsAny<RestaurantId>(), It.IsAny<CancellationToken>()), Times.Once());
            _eventInfoStorageMock.Verify(e => e.StoreEventAsync(It.IsAny<MenuCreatedEvent>(), It.Is<bool>(s => s == true), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPoicies.NUMBER_OF_RETRIES + 1));
        }

        private MenuCreatedEventHandler CreateHandler()
            => new(_restaurantRepositoryMock.Object, _eventInfoStorageMock.Object);

        private static MenuCreatedEvent CreateEvent()
            => new(new domain.Menus.ValueObjects.Identifiers.MenuId(1), new Name("test"), new RestaurantIdMenuId(2));
    }
}
