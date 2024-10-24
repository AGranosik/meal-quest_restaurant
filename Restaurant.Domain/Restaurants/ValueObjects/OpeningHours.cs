﻿using core.Extensions;
using domain.Common.BaseTypes;
using FluentResults;

namespace domain.Restaurants.ValueObjects
{
    public class OpeningHours : ValueObject<OpeningHours>
    {
        private const int WEEK_DAYS = 7;

        private List<WorkingDay> _workingDays = new();
        public IReadOnlyCollection<WorkingDay> WorkingDays => _workingDays;

        protected OpeningHours() { }

        public static Result<OpeningHours> Create(List<WorkingDay> workingDays)
        {
            var validatioNresult = Validation(workingDays);
            if (validatioNresult.IsFailed)
                return validatioNresult;

            return Result.Ok(new OpeningHours(workingDays));
        }

        protected OpeningHours(List<WorkingDay> workingDays)
        {
            _workingDays = workingDays;
        }

        private static Result Validation(List<WorkingDay> workingDays)
        {
            if (workingDays is null)
                return Result.Fail("");

            if (workingDays.Count != WEEK_DAYS)
                return Result.Fail("Not all days configured.");

            if (!workingDays.Select(wd => wd.Day).ToList().HasUniqueValues())
                return Result.Fail("Not all days are unique.");

            return Result.Ok();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            OpeningHours? other = obj as OpeningHours;
            return Enumerable.SequenceEqual(WorkingDays, other.WorkingDays);
        }
    }
}
