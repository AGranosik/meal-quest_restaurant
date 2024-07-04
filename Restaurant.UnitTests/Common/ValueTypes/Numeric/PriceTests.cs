using domain.Common.ValueTypes.Numeric;
using FluentAssertions;

namespace unitTests.Common.ValueTypes.Numeric
{
    [TestFixture]
    public class PriceTests
    {
        [Test]
        public void Creation_CannotBeNegative_ThrowsException()
        {
            var creation = () => new Price(-10);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Cannot0_ThrowsException()
        {
            var creation = () => new Price(0);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CanBePositive_Success()
        {
            var creation = () => new Price(1);
            creation.Should().NotThrow();
        }

        [Test]
        public void Creation_CannotBeMoreThan2DecimalPlacesPrecision_Success()
        {
            var creation = () => new Price(1.333333m);
            creation.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_CanHave2DecimalPlaces_Success()
        {
            var creation = () => new Price(1.33m);
            creation.Should().NotThrow();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var price = new Price(1.33m);
            (price == price).Should().BeTrue();
        }

        [Test]
        public void Equality_SameAmount_True()
        {
            var price = new Price(1.33m);
            var price2 = new Price(1.33m);
            (price == price2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentAmount_False()
        {
            var price = new Price(1.33m);
            var price2 = new Price(1.3m);
            (price == price2).Should().BeFalse();
        }

        [Test]
        public void Equality_SameValueWithPRovidedDifferentPrecision_False()
        {
            var price = new Price(1.30m);
            var price2 = new Price(1.3m);
            (price == price2).Should().BeTrue();
        }

        [Test]
        public void Equality_SameValueWithPRovidedDifferentPrecision_False2()
        {
            var price = new Price(1.00m);
            var price2 = new Price(1);
            (price == price2).Should().BeTrue();
        }
    }
}
