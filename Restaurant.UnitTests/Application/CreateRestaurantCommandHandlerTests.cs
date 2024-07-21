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

        [Test]
        public async Task Command_OwnerCannotBeNull_Fail()
        {
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(null, null);
            var result = await handler.Handle(command, CancellationToken.None);
            
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_OpeningHorusCannotBeNull_Fail()
        {
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(new CreateOwnerCommand(null, null, null), null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task Command_OwnerNameCannotBenullOrEmpty_Fail(string? ownerName)
        {
            var owner = new CreateOwnerCommand(ownerName, null, null);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, new OpeningHoursCommand(null!));
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }


        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task Command_OwnerSurnameCannotBenullOrEmpty_Fail(string? ownerSurname)
        {
            var owner = new CreateOwnerCommand("name", ownerSurname, null);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, new OpeningHoursCommand(null!));
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_OwnerAddressCannotBenull_Fail()
        {
            var owner = new CreateOwnerCommand("name", "surname", null);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, new OpeningHoursCommand(null!));
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task Command_OwnerSteetCannotBeNullOrEmpty_Fail(string? ownerSteet)
        {
            var address = new CreateAddressCommand(ownerSteet, null, 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, new OpeningHoursCommand(null!));
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task Command_CityNameCannotBeNullOrEmpty_Fail(string? ownerCity)
        {
            var address = new CreateAddressCommand("street", ownerCity, 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, new OpeningHoursCommand(null!));
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_WorkingDaysCanntBeEmpty_Fail()
        {
            var openingHours = new OpeningHoursCommand([]);
            var address = new CreateAddressCommand("street", "City", 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, openingHours);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        private CreateRestaurantCommandHandler CreateHandler()
            => new(_repositoryMock.Object);
    }
}
