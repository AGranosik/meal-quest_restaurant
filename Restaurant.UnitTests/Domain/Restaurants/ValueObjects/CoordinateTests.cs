using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects
{
    [TestFixture]
    public class CoordinateTests
    {
        [Test]
        public void Creation_CanBe0_Success()
        {
            var creation = () => new Coordinates(0, 0);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_CanBeNegative_Success()
        {
            var creation = () => new Coordinates(-10.2222222222, -15.4564567567756);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_CanBePositive_Success()
        {
            var creation = () => new Coordinates(10.2222222222, -15.4564567567756);
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_Reference_True()
        {
            var coordinates = new Coordinates(10.2222222222, -15.4564567567756);
            (coordinates == coordinates).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValues_True()
        {
            var coordinates = new Coordinates(10.2222222222, -15.4564567567756);
            var coordinates2 = new Coordinates(10.2222222222, -15.4564567567756);
            (coordinates == coordinates2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentXValues_False()
        {
            var coordinates = new Coordinates(10.2222222222, -15.4564567567756);
            var coordinates2 = new Coordinates(10.22222322222, -15.4564567567756);
            (coordinates == coordinates2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentYValues_False()
        {
            var coordinates = new Coordinates(10.2222222222, -15.4564567567756);
            var coordinates2 = new Coordinates(10.2222222222, 15.4564567567756);
            (coordinates == coordinates2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentValues_False()
        {
            var coordinates = new Coordinates(10.2222222222, -15.4564567567756);
            var coordinates2 = new Coordinates(-10.2222222222, 15.4564567567756);
            (coordinates == coordinates2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentValueAtLastDigit_False()
        {
            var coordinates = new Coordinates(51.86381570419643, 19.379230460861844);
            var coordinates2 = new Coordinates(51.86381570419644, 19.379230460861843);
            (coordinates == coordinates2).Should().BeFalse();
        }
    }
}
