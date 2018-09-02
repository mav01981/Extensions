using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Extensions
{
    public static class GenericExtension
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string[] PropertiesToStringArray<T>(this T @object)
        {
            return @object.GetType().GetProperties().Select(s => s.GetValue(@object).ToString()).ToArray();
        }

        public static string SerializeObjectToXML<T>(this T value) where T : class
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();

            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
            }

            return stringWriter.ToString();
        }

        public static List<T> EnumToList<T>(this T @object) where T : struct, IConvertible
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}
