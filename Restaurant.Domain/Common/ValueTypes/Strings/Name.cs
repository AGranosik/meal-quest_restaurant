using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Common.ValueTypes.Strings
{
    public class Name(string name) : SimpleValueType<NotEmptyString, Name>(name)
    {
    }
}
