using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers;

public class MenuId : SimpleValueType<int, MenuId>
{
    public MenuId(int value) : base(value)
    {
    }
}