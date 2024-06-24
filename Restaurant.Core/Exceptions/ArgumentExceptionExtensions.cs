using System.Runtime.CompilerServices;

namespace Restaurant.Core.Exceptions
{
    public static class ArgumentExceptionExtensions
    {
        public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> enumerable, [CallerArgumentExpression("enumerable")] string? paramName = null)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (!enumerable.Any())
                throw new ArgumentException(null, nameof(paramName));
        }
    }
}
