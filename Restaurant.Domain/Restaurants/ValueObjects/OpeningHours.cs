using domain.Common.BaseTypes;
using FluentResults;

namespace domain.Restaurants.ValueObjects
{
    // range and what day it include.
    public class OpeningHours : ValueObject<OpeningHours>
    {
        protected OpeningHours() { }

        public static Result<OpeningHours> Create(List<WorkingDay> workingDays)
        {
            //var validationResult = Validation(from, to);
            //if (validationResult.IsFailed)
            //    return validationResult;

            //return new OpeningHours(from, to);
        }

        protected OpeningHours(TimeOnly from, TimeOnly to)
        {
            From = from;
            To = to;
        }

        public TimeOnly From { get; }
        public TimeOnly To { get; }

        //get rid of exceptions
        // work on results.
        //private static Result Validation(List<WorkingDay> workingDays)
        //{
        //    if(workingDays.Count != 7)

        //    return Result.Ok();
        //}

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            OpeningHours? other = obj as OpeningHours;
            return other.From == From && other.To == To;
        }
    }
}
