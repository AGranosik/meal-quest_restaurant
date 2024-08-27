using core.SimpleTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Common.ValueTypes.Strings
{
    public class Name: SimpleValueType<NotEmptyString, Name>
    {
        public Name(string value) :base(value)
        {
            
        }
    }
}
