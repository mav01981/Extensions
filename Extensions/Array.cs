namespace Extensions
{
    using System;
    using System.Collections.Generic;

    public static class ArrayExtensions
    {
        public static string ToSingleString(this Array items, char split)
        {
            string @output = string.Empty;

            foreach (var item in items)
            {
                @output = @output + item + split;
            }

            return output.Substring(0, output.Length - 1);
        }

        public static List<T> ToList<T>(this Array items, Func<object, T> mapFunction)
        {
            if (items == null || mapFunction == null)
                return new List<T>();

            List<T> coll = new List<T>();
            for (int i = 0; i < items.Length; i++)
            {
                T val = mapFunction(items.GetValue(i));
                if (val != null)
                    coll.Add(val);
            }
            return coll;
        }

        public static T[] ConvertTo<T>(this Array ar)
        {
            T[] ret = new T[ar.Length];
            System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(ar.GetValue(0).GetType()))
            {
                for (int i = 0; i < ar.Length; i++)
                {
                    ret[i] = (T)tc.ConvertFrom(ar.GetValue(i));
                }
            }
            else
            {
                tc = System.ComponentModel.TypeDescriptor.GetConverter(ar.GetValue(0).GetType());
                if (tc.CanConvertTo(typeof(T)))
                {
                    for (int i = 0; i < ar.Length; i++)
                    {
                        ret[i] = (T)tc.ConvertTo(ar.GetValue(i), typeof(T));
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return ret;

        }

        public static T[] Shuffle<T>(this T[] list)
        {
            var r = new Random((int)DateTime.Now.Ticks);

            for (int i = list.Length - 1; i > 0; i--)
            {
                int j = r.Next(0, i - 1);
                var e = list[i];
                list[i] = list[j];
                list[j] = e;
            }
            return list;
        }
    }
}
