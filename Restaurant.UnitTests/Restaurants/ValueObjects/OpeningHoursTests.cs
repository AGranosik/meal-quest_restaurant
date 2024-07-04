using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace Restaurant.UnitTests.Restaurants.ValueObjects
{
    [TestFixture]
    public class OpeningHoursTests
    {
        [Test]
        public void Creation_FromEqualTo_Failure()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(12, 00));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_FromGeaterThanTo_Failure()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(13, 00), new TimeOnly(12, 00));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_ToGreaterThanFrom_Success()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(13, 00), new TimeOnly(13, 01));
            creationResult.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Creation_FromCannotHaveMicroSeconds_Failuere()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00, 12, 1, 12), new TimeOnly(13, 01));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_FromCannotHaveMiliSeconds_Failuere()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00, 12, 1), new TimeOnly(13, 01));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_FromCannotHaveSeconds_Failuere()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00, 12), new TimeOnly(13, 01, 12));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_ToCannotHaveMicroSeconds_Failuere()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 00, 12, 1, 12));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_ToCannotHaveMiliSeconds_Failuere()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 00, 12, 1));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_ToCannotHaveSeconds_Failuere()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 00, 12));
            creationResult.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Equality_SameReferenceTrue()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 01));
            var openingHours = creationResult.Value;
            (openingHours == openingHours).Should().BeTrue();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 01));
            var creationResult2 = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 01));
            var openingHours = creationResult.Value;
            var openingHours2 = creationResult2.Value;

            (openingHours == openingHours2).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentFrom_True()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 01));
            var creationResult2 = OpeningHours.Create(new TimeOnly(12, 01), new TimeOnly(13, 01));
            var openingHours = creationResult.Value;
            var openingHours2 = creationResult2.Value;

            (openingHours == openingHours2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentTo_True()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 01));
            var creationResult2 = OpeningHours.Create(new TimeOnly(12, 00), new TimeOnly(13, 11));
            var openingHours = creationResult.Value;
            var openingHours2 = creationResult2.Value;

            (openingHours == openingHours2).Should().BeFalse();
        }

        [Test]
        public void Equality_DifferentBoth_True()
        {
            var creationResult = OpeningHours.Create(new TimeOnly(12, 01), new TimeOnly(13, 01));
            var creationResult2 = OpeningHours.Create(new TimeOnly(12, 0), new TimeOnly(13, 11));
            var openingHours = creationResult.Value;
            var openingHours2 = creationResult2.Value;

            (openingHours == openingHours2).Should().BeFalse();
        }
    }
}
