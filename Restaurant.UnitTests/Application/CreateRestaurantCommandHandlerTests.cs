using application.Restaurants.Commands;
using FluentAssertions;
using infrastructure.Database.RestaurantContext.Repositories;
using Moq;

namespace unitTests.Application
{
    [TestFixture]
    internal class CreateRestaurantCommandHandlerTests
    {
        private IMock<IRestaurantRepository> _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRestaurantRepository>();
        }

        [Test]
        public void Creation_RepoCannotBeNull_ThrowsException()
        {
            var creation = () => new CreateRestaurantCommandHandler(null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_Success()
        {
            var creation = CreateHandler;
            creation.Should().NotThrow();
        }

        [Test]
        public async Task Command_CannotBeNull_Fail()
        {
            var handler = CreateHandler();
            var result = await handler.Handle(null, CancellationToken.None);
            result.IsFailed.Should().BeTrue();
        }

        private CreateRestaurantCommandHandler CreateHandler()
            => new(_repositoryMock.Object);
    }
}
