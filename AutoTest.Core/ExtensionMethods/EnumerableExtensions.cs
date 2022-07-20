namespace AutoTest.Core.ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => enumerable is null || !enumerable.Any();

        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable) => !IsEmpty(enumerable);
    }
}
