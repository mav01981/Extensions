using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class CollectionsExtensions
    {
        public static IEnumerable ClassToIEnumerable<T>(this T @object)
        {
            return @object.GetType().GetProperties().Select(s => s.GetValue(@object).ToString()).ToArray();
        }
        public static IEnumerable Append(this IEnumerable first, params object[] second)
        {
            return first.OfType<object>().Concat(second);
        }
        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second)
        {
            return first.Concat(second);
        }
        public static IEnumerable Prepend(this IEnumerable first, params object[] second)
        {
            return second.Concat(first.OfType<object>());
        }
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> first, params T[] second)
        {
            return second.Concat(first);
        }
        public static IEnumerable<string> Compare(this string[] value1, string[] value2)
        {
            return value1.Except(value2);
        }
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;

            return list;
        }
    }
}
