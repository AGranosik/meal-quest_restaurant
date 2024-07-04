using domain.Common.BaseTypes;
using FluentResults;

namespace domain.Restaurants.ValueObjects
{
    // it will get more complicated like exceptions days
    // on which day when
    public class OpeningHours : ValueObject<OpeningHours>
    {
        
        public static Result<OpeningHours> Create(TimeOnly from, TimeOnly to)
        {
            var validationResult = Validation(from, to);
            if (validationResult.IsFailed)
                return validationResult;

            return new OpeningHours(from, to);
        }

        private OpeningHours(TimeOnly from, TimeOnly to)
        {
            From = from;
            To = to;
        }

        public TimeOnly From { get; }
        public TimeOnly To { get; }

        //get rid of exceptions
        // work on results.
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

            OpeningHours? other = obj as OpeningHours;
            return other.From == From && other.To == To;
        }
    }
}
