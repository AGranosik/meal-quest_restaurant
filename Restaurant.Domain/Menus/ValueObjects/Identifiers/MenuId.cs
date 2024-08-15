using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers
{

    public class MenuId(int value) : SimpleValueType<int, MenuId>(value)
    {
    }
}
