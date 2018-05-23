using System;
using System.IO;

namespace Extensions
{
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
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
