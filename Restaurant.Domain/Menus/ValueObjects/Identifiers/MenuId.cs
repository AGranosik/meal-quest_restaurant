using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Menus.ValueObjects.Identifiers
{

    public class MenuId(int id) : SimpleValueType<int, MenuId>(id)
    {
    }
}
