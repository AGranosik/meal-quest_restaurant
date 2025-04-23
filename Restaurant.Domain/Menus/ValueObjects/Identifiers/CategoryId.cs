using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers;

public sealed class CategoryId : SimpleValueType<int, CategoryId>
{
    public CategoryId(int value) : base(value)
    {
    }
}