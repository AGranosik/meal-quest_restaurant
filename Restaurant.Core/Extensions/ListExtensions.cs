namespace core.Extensions
{
    public static class ListExtensions
    {
        public static bool HasUniqueValues<T>(this List<T> list)
            => list.GroupBy(x => x).Count() == list.Count;

    }
}
