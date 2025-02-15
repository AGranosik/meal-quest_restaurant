using application.EventHandlers;
using application.Restaurants.Commands;
using application.Restaurants.Commands.Interfaces;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace unitTests.Application.Restaurants
{
    [TestFixture]
    internal class CreateRestaurantCommandHandlerTests
    {
        private Mock<IRestaurantRepository> _repositoryMock;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<CreateRestaurantCommandHandler>> _loggerMock;
        private const string _validName = "test";

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRestaurantRepository>();
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        }

        [Test]
        public void Creation_RepoCannotBeNull_ThrowsException()
        {
            var creation = () => new CreateRestaurantCommandHandler(null!, null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }


        [Test]
        public void Creation_MeidatorCannotBeNull_ThrowsException()
        {
            var creation = () => new CreateRestaurantCommandHandler(_repositoryMock.Object, _mediatorMock.Object, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_LoggerCannotBeNull_ThrowsException()
        {
            var creation = () => new CreateRestaurantCommandHandler(_repositoryMock.Object, null!, null!);
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
            var result = await handler.Handle(null!, CancellationToken.None);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_NameCannotBeNull_Fail()
        {
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(null, null, null, null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_OwnerCannotBeNull_Fail()
        {
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, null, null, null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_OpeningHoursCannotBeNull_Fail()
        {
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, new CreateOwnerCommand(null, null, null), null, null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task Command_OwnerNameCannotBeNullOrEmpty_Fail(string? ownerName)
        {
            var owner = new CreateOwnerCommand(ownerName, null, null);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, owner, new OpeningHoursCommand(null!), null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }


        [Test]
        [TestCase(null)]
        [TestCase("")]
        public async Task Command_OwnerSurnameCannotBeNullOrEmpty_Fail(string? ownerSurname)
        {
            var owner = new CreateOwnerCommand("name", ownerSurname, null);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, owner, new OpeningHoursCommand(null!), null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_OwnerAddressCannotBenull_Fail()
        {
            var owner = new CreateOwnerCommand("name", "surname", null);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, owner, new OpeningHoursCommand(null!), null);
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
            var command = new CreateRestaurantCommand(_validName, owner, new OpeningHoursCommand(null!), null);
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
            var command = new CreateRestaurantCommand(_validName, owner, new OpeningHoursCommand(null!), null);
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
            var command = new CreateRestaurantCommand(_validName, owner, openingHours, null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Command_AddressCannotBeNull_Fail()
        {
            var openingHours = new OpeningHoursCommand(
            [
                new(DayOfWeek.Monday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Tuesday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Wednesday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Thursday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Friday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Saturday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Sunday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
            ]);
            var address = new CreateAddressCommand("street", "City", 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, owner, openingHours, null);
            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Coomand_PassedToRepoWhenValid_Success()
        {
            var openingHours = new OpeningHoursCommand(
            [
                new(DayOfWeek.Monday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Tuesday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Wednesday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Thursday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Friday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Saturday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Sunday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
            ]);
            var address = new CreateAddressCommand("street", "City", 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, owner, openingHours, address);
            var result = await handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(m => m.CreateAsync(It.IsAny<Restaurant>(), It.IsAny<CancellationToken>()), Times.Once());
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task Command_NotificationHandlerCalled_Success()
        {
            var openingHours = new OpeningHoursCommand(
            [
                new(DayOfWeek.Monday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Tuesday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Wednesday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Thursday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Friday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Saturday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
                new(DayOfWeek.Sunday, new DateTime(12, 12, 12, 11, 00, 00, DateTimeKind.Utc),new DateTime(12, 12, 12, 13, 00, 00, DateTimeKind.Utc)),
            ]);
            var address = new CreateAddressCommand("street", "City", 0, 0);
            var owner = new CreateOwnerCommand("name", "surname", address);
            var handler = CreateHandler();
            var command = new CreateRestaurantCommand(_validName, owner, openingHours, address);
            var result = await handler.Handle(command, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            _mediatorMock.Verify(m => m.Publish(It.IsAny<AggregateChangedEvent<Restaurant, RestaurantId>>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        private CreateRestaurantCommandHandler CreateHandler()
            => new(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
    }
}
