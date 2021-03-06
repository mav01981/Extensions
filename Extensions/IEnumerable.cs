﻿namespace Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    /// <summary>
    /// Linq to Memory.
    /// </summary>
    public static class EnumerableExtensions
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
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
        {
            sortExpression += "";
            string[] parts = sortExpression.Split(' ');
            bool descending = false;
            string property = "";

            if (parts.Length > 0 && parts[0] != "")
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("esc");
                }

                PropertyInfo prop = typeof(T).GetProperty(property);

                if (prop == null)
                {
                    throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
                }

                if (descending)
                    return list.OrderByDescending(x => prop.GetValue(x, null));
                else
                    return list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }

        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.ToDictionary(group => group.Key, group => group.ToList());
        }

        public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> list, Func<T, int> Predicate)
        {
            var dict = new Dictionary<int, T>();

            foreach (var item in list)
            {
                if (!dict.ContainsKey(Predicate(item)))
                {
                    dict.Add(Predicate(item), item);
                }
            }

            return dict.Values.AsEnumerable();
        }

        public static T Aggregate<T>(this IEnumerable<T> list, Func<T, T, T> aggregateFunction)
        {
            return Aggregate<T>(list, default(T), aggregateFunction);
        }

        public static T Aggregate<T>(this IEnumerable<T> list, T defaultValue,
            Func<T, T, T> aggregateFunction)
        {
            return list.Count() <= 0 ?
                defaultValue : list.Aggregate<T>(aggregateFunction);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
        {
            return en.Skip(page * pageSize).Take(pageSize);
        }

        public static IEnumerable Enum(this IEnumerable @objectArray)
        {
            if (@objectArray != null)
            {
                foreach (var t in @objectArray)
                {
                    yield return t;
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
