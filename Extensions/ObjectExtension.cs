using System;
using System.Collections;

namespace Extensions
{
    public static class ObjectExtension
    {
        private static string FlattenModel(Type type, object @object, ref string output)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    output = output + property.Name + ":" + property.GetValue(@object, null).ToString() + " ";
                }
                else if (property.PropertyType.IsArray)
                {
                    if (property.PropertyType.GetElementType().IsValueType || property.PropertyType.GetElementType() == typeof(string))
                    {
                        IEnumerable items = property.GetValue(@object) as IEnumerable;
                        output = output + property.Name + ":";

                        foreach (var item in items.Enum())
                        {
                            output = output + item.ToString();
                        }
                    }
                    else
                    {
                        IEnumerable items = property.GetValue(@object) as IEnumerable;

                        foreach (var item in items.Enum())
                        {
                            if (item != null)
                            {
                                output = output + "|" + item.GetType().ToString() + "|";
                                FlattenModel(item.GetType(), item, ref output);
                            }
                        }
                    }
                }
                else if (property.GetValue(@object) is IList && property.GetValue(@object).GetType().IsGenericType)
                {
                    IEnumerable items = property.GetValue(@object) as IEnumerable;

                    foreach (var item in items.Enum())
                    {
                        if (item != null)
                        {
                            output = output + "|" + item.GetType().ToString() + "|";
                            FlattenModel(item.GetType(), item, ref output);
                        }
                    }
                }
                else
                {
                    if (property.GetValue(@object, null) != null)
                    {
                        output = output + "|" + type.ToString() + "|";
                        FlattenModel(property.PropertyType, property.GetValue(@object, null), ref output);
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Flatten Object to String.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string ObjectToString<T>(this T @object)
        {
            string output = string.Empty;

            Type type = @object.GetType();

            var properties = type.GetProperties();

            FlattenModel(type, @object, ref output);

            return output;
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
