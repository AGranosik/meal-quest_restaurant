using core.Operations.Numeric;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace domain.Common.ValueTypes.Numeric;

public class Price : SimpleValueType<decimal, Price>
{
    public Price(decimal amount) : base(amount)
    {
        Walidacja(amount);
    }

    private static void Walidacja(decimal amount)
    {
        if(amount <= 0)
            throw new ArgumentException(nameof(amount));

        if(!amount.IsRoundedToSpecificPrecision(2))
            throw new ArgumentException(nameof(amount));
    }
}