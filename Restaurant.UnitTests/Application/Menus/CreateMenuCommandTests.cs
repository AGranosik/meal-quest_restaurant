using application.Menus.Commands;
using application.Menus.Commands.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace unitTests.Application.Menus;

[TestFixture]
internal class CreateMenuCommandTests
{
    private Mock<IMenuRepository> _repositoryMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<ILogger<CreateMenuCommandHandler>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IMenuRepository>();
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<CreateMenuCommandHandler>>();
    }

    [Test]
    public void Creation_RepoCannotBeNull_ThrowsException()
    {
        var creation = () => new CreateMenuCommandHandler(null!, null!, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_MediatorCannotBeNull_ThrowsException()
    {
        var creation = () => new CreateMenuCommandHandler(_repositoryMock.Object, null!, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Creation_LoggerCannotBeNull_ThrowsException()
    {
        var creation = () => new CreateMenuCommandHandler(_repositoryMock.Object, _mediatorMock.Object, null!);
        creation.Should().Throw<ArgumentException>();
    }

    [Test]
    public async Task Creation_CommandCannotBeNull_Fail()
    {
        var handler = new CreateMenuCommandHandler(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
        var result = await handler.Handle(null!, CancellationToken.None);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public async Task Creation_GroupsCannotBeNull_Fail()
    {
        var handler = new CreateMenuCommandHandler(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
        var result = await handler.Handle(new CreateMenuCommand(null, null!, default), CancellationToken.None);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public async Task Creation_MealsCannotBeNull_Fail()
    {
        var handler = new CreateMenuCommandHandler(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
        var result = await handler.Handle(new CreateMenuCommand(null,
                [
                    new CreateGroupCommand(null, null!)
                ],
                default),
            CancellationToken.None);
        result.IsFailed.Should().BeTrue();
    }

    [Test]
    public async Task Creation_IngredientsCannotBeNull_Fail()
    {
        var handler = new CreateMenuCommandHandler(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
        var result = await handler.Handle(new CreateMenuCommand(null,
        [
            new CreateGroupCommand(null, [
                new(null, 0, null!)
            ])
        ], default), CancellationToken.None);
        result.IsFailed.Should().BeTrue();
    }
}