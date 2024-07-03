namespace core.Operations.Numeric
{
    public static class RoundingExtensions
    {
        public static bool IsRoundedToSpecificPrecision(this decimal value, int precision)
            => Math.Round(value, precision) == value;
    }
}
