using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Extensions
{
    public static class CSVExtensions
    {
        public static string ToCSV<T>(this IEnumerable<T> rows, string separator)
        {
            StringBuilder csv = new StringBuilder();

            try
            {
                if (rows != null)
                {
                    foreach (var header in rows)
                    {
                        var properties = header.GetType().GetProperties();

                        foreach (var prop in properties)
                        {
                            if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(int))
                            {
                                csv = csv.AppendFormat($"{prop.Name}{separator}");
                            }
                            else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                            {
                                csv = csv.AppendFormat($"{prop.Name}{separator}");
                            }
                        }
                        break;
                    }
                    csv.AppendLine();

                    foreach (var row in rows)
                    {
                        var properties = row.GetType().GetProperties();

                        foreach (var prop in properties)
                        {
                            object value = GetPropValue(row, prop.Name);

                            if (value == null)
                            {
                                csv = csv.AppendFormat($"#{separator}");
                            }
                            else if (value.GetType() == typeof(string) || value.GetType() == typeof(int))
                            {
                                csv = csv.AppendFormat($"{value}{separator}");
                            }
                            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                            {

                                string split = string.Empty;
                                var listOfItems = value as IList;
                                foreach (var t in Flatten(listOfItems))
                                {
                                    Type itemType = value.GetType().GetGenericArguments()[0];

                                    if (itemType == typeof(int))
                                    {
                                        split = split + $"{t} |";
                                    }
                                    else
                                    {
                                        var items = t.GetType().GetProperties();

                                        foreach (var i in items)
                                        {
                                            object val = GetPropValue(t, i.Name);

                                            var isList = val as IList;

                                            if (isList == null)
                                            {
                                                split = split + $"{val} |";
                                            }
                                            else
                                            {
                                                foreach (var v in isList)
                                                {
                                                    split = split + $"{v} |";
                                                }
                                            }
                                        }
                                    }
                                }
                                csv = csv.AppendFormat($"{split}{separator}");
                            }
                        }
                        csv.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return csv.ToString();
        }

        internal static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        static IEnumerable Flatten(IList enumerable)
        {
            foreach (object element in enumerable)
            {
                IList candidate = element as IList;
                if (candidate != null)
                {
                    foreach (object nested in Flatten(candidate))
                    {
                        yield return nested;
                    }
                }
                else
                {
                    yield return element;
                }
            }
        }

        internal static string BuildString(IList list)
        {
            string collection = string.Empty;
            foreach (var select in list)
            {
                collection = collection.Length == 0 ? collection + select + " " : collection + select + " ";
            }
            if (collection.Length >= 1)
                collection = collection.Remove(collection.Length - 1, 1);

            return collection;
        }
    }
}

