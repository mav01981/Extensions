using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Extensions
{
    public static class CSVExtension
    {
        public static string ToCSV<T>(this IEnumerable<T> list, string separator)
        {
            StringBuilder csv = new StringBuilder();

            if (list != null)
            {
                foreach (var item in list)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                        {
                            object propValue = property.GetValue(item, null);

                            var _list = propValue as IList;

                            Type listType = (_list[0].GetType());

                            if ((typeof(int) == listType) || (typeof(string) == listType))
                            {
                                csv = csv.AppendFormat($"{property.Name}{separator}");
                            }
                            else
                            {
                                var sub = _list[0].GetType().GetProperties();
                                csv = csv.AppendFormat($"{sub[0].Name}{separator}");
                            }
                        }
                        else
                        {
                            csv = csv.AppendFormat($"{property.Name}{separator}");
                        }
                    }
                    break;
                }

                csv.AppendLine();

                foreach (var item in list)
                {
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                        {
                            object propValue = property.GetValue(item, null);

                            var _list = propValue as IList;

                            Type listType = (_list[0].GetType());

                            if ((typeof(int) == listType) || (typeof(string) == listType))
                            {
                                var selections = propValue as IList;
                                csv = csv.AppendFormat($"{BuildString(selections)}{separator}");
                            }
                            else
                            {
                                var sub = _list[0].GetType().GetProperties();

                                if (sub.Length == 1)
                                {
                                    object nestedProp = GetPropValue(_list[0], sub[0].Name);

                                    var nestedselections = nestedProp as IList;
                                    csv = csv.AppendFormat($"{BuildString(nestedselections)}{separator}");
                                }
                            }
                        }
                        else
                        {
                            csv = csv.AppendFormat($"{property.Name}{separator}");
                        }
                    }
                }
            }
            return csv.ToString();
        }

        internal static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        internal static string BuildString(IList list)
        {
            string collection = string.Empty;
            foreach (var select in list)
            {
                collection = collection.Length == 0 ? collection + select + "," : collection + select + ",";
            }
            collection = collection.Remove(collection.Length - 1, 1);

            return collection;
        }
    }
}

