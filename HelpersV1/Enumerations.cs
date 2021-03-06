﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpersV1
{
    public static class Enumerations
    {
        public static List<T> EnumToList<T>(Type @object) 
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

        public static IEnumerable<T> EnumToEnumerable<T>(Type @object)
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
