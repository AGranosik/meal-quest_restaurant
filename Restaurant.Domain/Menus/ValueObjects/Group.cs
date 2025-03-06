using core.Extensions;
using domain.Common.BaseTypes;
using domain.Common.ValueTypes.Strings;
using FluentResults;

namespace domain.Menus.ValueObjects;

public class Group : ValueObject<Group>
{
    public static Result<Group> Create(List<Meal> meals, Name groupName)
    {
        var creationResult = CreationValidation(meals, groupName);
        if (creationResult.IsFailed)
            return creationResult;

        return Result.Ok(new Group(meals, groupName));
    }
    private Group() { }
    private Group(List<Meal> meals, Name groupName)
    {
        Meals = meals;
        GroupName = groupName;
    }

    public List<Meal> Meals { get; }
    public Name GroupName { get; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is not Group other) return false;
        return GroupName! == other.GroupName! && Meals!.SequenceEqual(other.Meals!);
    }

    private static Result CreationValidation(List<Meal> meals, Name groupName)
    {
        if (meals is null)
            return Result.Fail("Meals cannot be null.");

        if (meals.Count == 0)
            return Result.Fail("Meals cannot be empty.");

        if (groupName is null)
            return Result.Fail("Group name cannot be nul.");

        if (!meals.HasUniqueValues())
            return Result.Fail("Meals have to be unique.");

        return Result.Ok();
    }
}