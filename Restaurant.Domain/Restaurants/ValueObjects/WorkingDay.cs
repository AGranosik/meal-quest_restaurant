using domain.Common.BaseTypes;
using FluentResults;

namespace domain.Restaurants.ValueObjects
{
    public class WorkingDay : ValueObject<WorkingDay>
    {

        public static Result<WorkingDay> Create(DayOfWeek day, TimeOnly from, TimeOnly to)
        {
            var validatioNResult = Validation(from, to);
            if (validatioNResult.IsFailed)
                return validatioNResult;

            return new WorkingDay(day, from, to);
        }

        protected WorkingDay() { }
        protected WorkingDay(DayOfWeek day, TimeOnly from, TimeOnly to)
        {
            Day = day;
            From = from;
            To = to;
        }

        public DayOfWeek Day { get; }
        public TimeOnly From { get; }
        public TimeOnly To { get; }

        private static Result Validation(TimeOnly from, TimeOnly to)
        {
            if (from.Microsecond != 0 || to.Millisecond != 0)
                return Result.Fail("Specify time without microseconds.");

            if (from.Millisecond != 0 || to.Millisecond != 0)
                return Result.Fail("Specify time without ms.");

            if (from.Second != 0 || to.Second != 0)
                return Result.Fail("Specify time without ms.");

            if (from >= to)
                return Result.Fail("From cannot be greater than to.");

            return Result.Ok();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            WorkingDay? other = obj as WorkingDay;
            return Day == other.Day && other.From == From && other.To == To;
        }
    }
}
