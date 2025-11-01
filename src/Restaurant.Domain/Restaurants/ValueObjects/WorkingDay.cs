using domain.Common.DomainImplementationTypes;
using FluentResults;

namespace domain.Restaurants.ValueObjects;

public class WorkingDay : ValueObject<WorkingDay>
{

    public static Result<WorkingDay> Create(DayOfWeek day, TimeOnly from, TimeOnly to)
    {
        var validationResult = Validation(from, to);
        if (validationResult.IsFailed)
            return validationResult;

        return new WorkingDay(day, from, to, false);
    }

    public static Result<WorkingDay> FreeDay(DayOfWeek day)
        => new WorkingDay(day, default, default, true);

    public bool IsFreeDay()
        => Free;

    protected WorkingDay() { }
    protected WorkingDay(DayOfWeek day, TimeOnly from, TimeOnly to, bool free)
    {
        Day = day;
        From = from;
        To = to;
        Free = free;
    }

    public DayOfWeek Day { get; }
    public TimeOnly From { get; }
    public TimeOnly To { get; }
    public bool Free { get; } = false;


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

        var other = obj as WorkingDay;
        return other is not null && Day == other.Day! && other.From == From && other.To == To;
    }
}