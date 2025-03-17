using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using domain.Common.ValueTypes.Strings;
using FluentResults;

namespace domain.Menus.ValueObjects;

public class Category : SimpleValueType<NotEmptyString, Category>
{
    public Category(NotEmptyString value) : base(value)
    {
    }
}