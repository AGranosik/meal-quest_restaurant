using application.Restaurants.Commands;
using domain.Restaurants.Aggregates;
using FluentAssertions;
using infrastructure.Database.RestaurantContext.Repositories;
using Moq;

namespace unitTests.Application
{
    [TestFixture]
    internal class CreateRestaurantCommandHandlerTests
    {
        private Mock<IRestaurantRepository> _repositoryMock;

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
        public async Task Command_WorkingDaysCannotBeEmpty_Fail()
        {
            var openingHours = new OpeningHoursCommand([]);
            var address = new CreateAddressCommand("street", "City", 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, openingHours);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Coomand_PassedToRepoWhenValid_Success()
        {
            var openingHours = new OpeningHoursCommand(
            [
                new(DayOfWeek.Monday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Tuesday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Wednesday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Thursday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Friday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Saturday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
                new(DayOfWeek.Sunday, new DateTime(12, 12, 12, 11, 00, 00),new DateTime(12, 12, 12, 13, 00, 00)),
            ]);
            var address = new CreateAddressCommand("street", "City", 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(owner, openingHours);
            var result = await handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(m => m.CreateAsync(It.IsAny<Restaurant>(), It.IsAny<CancellationToken>()), Times.Once());
            result.IsSuccess.Should().BeTrue();
        }

        private CreateRestaurantCommandHandler CreateHandler()
            => new(_repositoryMock.Object);
    }
}
