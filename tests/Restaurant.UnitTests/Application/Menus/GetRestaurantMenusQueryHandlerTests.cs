using application.Menus.Queries;
using application.Menus.Queries.Dto;
using application.Menus.Queries.Interfaces;
using FluentAssertions;
using Moq;
using sharedTests;

namespace unitTests.Application.Menus;

[TestFixture]
public sealed class GetRestaurantMenusQueryHandlerTests
{
    private Mock<IReadMenuRepository> _repositoryMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IReadMenuRepository>();
    }
    
    [Test]
    public void Creation_RepoCannotBeNull_ThrowsArgumentNullException()
    {
        var creation = () => new GetRestaurantMenusQueryHandler(null!);
        creation.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Creation_Success()
    {
        var creation = CreateHandler;
        creation.Should().NotThrow();
    }

    [Test]
    public async Task Handle_RepoThrows_Throws()
    {
        var handler = CreateHandler();
        var cancellationToken = TestContext.CurrentContext.CancellationToken;
        _repositoryMock.Setup(r => r.GetRestaurantMenu(It.IsAny<int>(), cancellationToken))
            .ThrowsAsync(new TestException());
        
        var action = () => handler.Handle(CreateQuery(), cancellationToken);
        await action.Should().ThrowAsync<TestException>();
    }

    [Test]
    public async Task Handle_ReturnsData_Success()
    {
        var handler = CreateHandler();
        var cancellationToken = TestContext.CurrentContext.CancellationToken;
        var returnData = new MenuRestaurantDto("some-name", Array.Empty<MenuGroupDto>().ToList());

        _repositoryMock.Setup(r => r.GetRestaurantMenu(It.IsAny<int>(), cancellationToken))
            .ReturnsAsync(returnData);
        
        var result = await handler.Handle(CreateQuery(), cancellationToken);
        result.Should().NotBeNull();
        result.Should().Be(returnData);
    }

    private GetRestaurantMenusQueryHandler CreateHandler() => new(_repositoryMock.Object);
    
    private static GetRestaurantMenusQuery CreateQuery() => new(1);
}