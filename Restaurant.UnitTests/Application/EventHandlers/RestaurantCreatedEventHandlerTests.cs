using application.EventHandlers.Restaurants;
using application.Menus.Commands.Interfaces;
using FluentAssertions;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public class RestaurantCreatedEventHandlerTests
    {
        private Mock<IMenuRepository> _menuRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _menuRepositoryMock = new Mock<IMenuRepository>();
        }

        [Test]
        public void Creation_MediatorCannotBeNull_ThrowsException()
        {
            var creation = () => new RestaurantCreatedEventHandler(null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task RestaurantCreatedEvent_EventIsNull_ThrowsException()
        {
            var handler = CreateEventHandler();
            var action = () => handler.Handle(null, CancellationToken.None);
            await action.Should().ThrowAsync<Exception>();
        }

        private RestaurantCreatedEventHandler CreateEventHandler()
            => new(_menuRepositoryMock.Object);
    }
}
