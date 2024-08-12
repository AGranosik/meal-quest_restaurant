using System.Text.RegularExpressions;
using application.Menus.Commands;
using application.Menus.Commands.Interfaces;
using FluentAssertions;
using Moq;

namespace unitTests.Application.Menus
{
    [TestFixture]
    internal class CreateMenuCommandTests
    {
        private Mock<IMenuRepository> _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IMenuRepository>();
        }

        [Test]
        public void Creation_RepoCannotBeNull_ThrowsException()
        {
            var creation = () => new CreateMenuCommandHandler(null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public async Task Creation_CommandCannotBeNull_Fail()
        {
            var handler = new CreateMenuCommandHandler(_repositoryMock.Object);
            var result = await handler.Handle(null, CancellationToken.None);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Creation_GroupsCannotBeNull_Fail()
        {
            var handler = new CreateMenuCommandHandler(_repositoryMock.Object);
            var result = await handler.Handle(new CreateMenuCommand(null, null), CancellationToken.None);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Creation_MealsCannotBeNull_Fail()
        {
            var handler = new CreateMenuCommandHandler(_repositoryMock.Object);
            var result = await handler.Handle(new CreateMenuCommand(null,
            [
                new CreateGroupCommand(null, null)
            ]), CancellationToken.None);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public async Task Creation_IngredientsCannotBeNull_Fail()
        {
            var handler = new CreateMenuCommandHandler(_repositoryMock.Object);
            var result = await handler.Handle(new CreateMenuCommand(null,
            [
                new CreateGroupCommand(null, [
                   new(null, 0, null)
                ])
            ]), CancellationToken.None);
            result.IsFailed.Should().BeTrue();
        }
    }
}
