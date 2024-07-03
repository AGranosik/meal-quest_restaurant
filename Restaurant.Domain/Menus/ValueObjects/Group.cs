using core.Exceptions;
using core.Extensions;
using Restaurant.Domain.Common.BaseTypes;
using Restaurant.Domain.Common.ValueTypes.Strings;

namespace Restaurant.Domain.Menus.ValueObjects
{
    public class Group : ValueObject<Group>
    {
        public Group(List<Meal> meals, Name groupName)
        {
            CreationValidation(meals, groupName);
            Meals = meals;
            GroupName = groupName;
        }

        public List<Meal> Meals { get; }
        public Name GroupName { get; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Group other = obj as Group;
            if (other == null) return false;
            return GroupName == other.GroupName && Meals.SequenceEqual(other.Meals);
        }

        private static void CreationValidation(List<Meal> meals, Name groupName)
        {
            ArgumentExceptionExtensions.ThrowIfNullOrEmpty(meals, nameof(meals));
            ArgumentNullException.ThrowIfNull(groupName, nameof(groupName));

            if(!meals.HasUniqueValues())
                throw new ArgumentException(nameof(meals));
        }
    }
}
