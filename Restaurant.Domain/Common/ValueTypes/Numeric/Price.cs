using Restaurant.Core.Operations.Numeric;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Common.ValueTypes.Numeric
{
    public class Price : ValueObject<Price>
    {
        public Price(decimal amount)
        {
            Walidacja(amount);
            Amount = amount;
        }

        public decimal Amount { get; }

        private static void Walidacja(decimal amount)
        {
            if(amount <= 0)
                throw new ArgumentException(nameof(amount));

            if(!amount.IsRoundedToSpecificPrecision(2))
                throw new ArgumentException(nameof(amount));
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Price? other = obj as Price;
            if (other == null) return false;
            return Amount == other.Amount;
        }
    }
}
