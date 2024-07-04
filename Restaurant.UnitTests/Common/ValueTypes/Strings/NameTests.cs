using domain.Common.ValueTypes.Strings;
using FluentAssertions;

namespace unitTests.Common.ValueTypes.Strings
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void Creation_CannotBeNull_ThrowsException()
        {
            var creation = () => new Name(null);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CannotBeEmpty_ThrowsException()
        {
            var creation = () => new Name(string.Empty);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CannotBeWhiteSpaces_ThrowsException()
        {
            var creation = () => new Name("         ");
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_SomeValue_ThrowsException()
        {
            var creation = () => new Name("asdasd");
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var name = new Name("asdasd");
            (name == name).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentValues_False()
        {
            var name = new Name("asdasd");
            var name2 = new Name("asdasd2");
            (name2 == name).Should().BeFalse();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var name = new Name("asdasd");
            var name2 = new Name("asdasd");
            (name2 == name).Should().BeTrue();
        }
    }
}
