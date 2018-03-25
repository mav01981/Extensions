using System;
using System.Collections.Generic;
namespace Extensions
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XMLExtensions
    {
        public static string ToXML<T>(this T value) where T : class
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();

            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
            }

            return stringWriter.ToString();
        }
    }
}
