using domain.Restaurants.ValueObjects;
using FluentAssertions;

namespace unitTests.Domain.Restaurants.ValueObjects
{
    [TestFixture]
    internal class OpeningHoursTests
    {
        [Test]
        public void Creation_WorkingDaysCannotBeNull_Failure()
        {
            var creatioNReuslt = OpeningHours.Create(null);
            creatioNReuslt.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_WorkingDaysCannotBeEmpty_Failure()
        {
            var creatioNReuslt = OpeningHours.Create(new List<WorkingDay>());
            creatioNReuslt.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_WeekNotComplete_Failure()
        {
            var creatioNReuslt = OpeningHours.Create(new List<WorkingDay>
            {
                WorkingDay.Create(DayOfWeek.Monday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.FreeDay(DayOfWeek.Tuesday).Value,
                WorkingDay.Create(DayOfWeek.Wednesday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Thursday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Sunday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
            });
            creatioNReuslt.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_WeekNotUnique_Failure()
        {
            var creatioNReuslt = OpeningHours.Create(new List<WorkingDay>
            {
                WorkingDay.Create(DayOfWeek.Monday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.FreeDay(DayOfWeek.Tuesday).Value,
                WorkingDay.Create(DayOfWeek.Wednesday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Thursday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Sunday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
            });
            creatioNReuslt.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_Success()
        {
            var creatioNReuslt = OpeningHours.Create(new List<WorkingDay>
            {
                WorkingDay.Create(DayOfWeek.Monday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.FreeDay(DayOfWeek.Tuesday).Value,
                WorkingDay.Create(DayOfWeek.Wednesday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Thursday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Saturday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Sunday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
            });
            creatioNReuslt.IsSuccess.Should().BeTrue();
        }
    }
}
