namespace Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public static class StringExtension
    {
        public static string RemoveLastCharacter(this String @string)
        {
            return @string.Substring(0, @string.Length - 1);
        }
        public static string RemoveLast(this String @string, int number)
        {
            return @string.Substring(0, @string.Length - number);
        }
        public static string RemoveFirstCharacter(this String @string)
        {
            return @string.Substring(1);
        }
        public static string RemoveFirst(this String @string, int number)
        {
            return @string.Substring(number);
        }
        public static MemoryStream ToStream(this string str)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(str);
                    writer.Flush();
                }

                stream.Position = 0;

                return stream;
            };

        }
        public static T DeserializeXMLToObject<T>(this string xml)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(xml)) return default(T);

            try
            {
                using (StreamReader xmlStream = new StreamReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    returnObject = (T)serializer.Deserialize(xmlStream);
                }
            }
            catch (Exception)
            {
                throw new Exception("XML is in incorrect format.");
            }

            return returnObject;
        }
        public static string RemoveInvalidSaveCharacters(this string value)
        {
            string[] fileExtensions = { "zip", "txt", "xls", "xlsx", "doc", "docx", "jpg", "png" };
            char[] illegal = { '*', '.', '/', '\\', ';', ':', '?', '|', '=', ',', '<', '>' };

            var file = fileExtensions.Where(x => value.Contains(x)).ToList();

            if (file.Count() > 0)
            {
                illegal
                    .ToList().ForEach(x => value = value.Replace(x, ' ').Trim());

                return value.Replace(file.First(), "").Replace(" ", "") + $".{file.First()}";
            }

            return string.Empty;
        }
    }
}
