using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers;

public sealed class MenuId : SimpleValueType<int, MenuId>
{
    public MenuId(int value) : base(value)
    {
    }
}