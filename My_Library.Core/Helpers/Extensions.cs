using System;
using System.Collections.Generic;
using System.Linq;

namespace My_Library.Core.Helpers
{
    public static partial class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> instance) where T : class
        {
            return instance == null || instance.Any() == false;
        }

        public static string ToLocalize(this bool value)
        {
            return value ? "Evet" : "Hayır";
        }

        public static dynamic ToDynamicDictionary<TValue>(this IDictionary<string, TValue> dictionary)
        {
            return new DynamicDictionary<TValue>(dictionary);
        }
    }

    public static partial class Extensions
    {
        public static string GetDescriptionFromEnumValue(this Enum value)
        {
            return Utility.GetDescriptionFromEnumValue(value);
        }


        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
        }

        public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
        {
            return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
        }

        private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
        {
            ulong bits = Convert.ToUInt64(value);
            List<Enum> results = new List<Enum>();
            for (int i = values.Length - 1; i >= 0; i--)
            {
                ulong mask = Convert.ToUInt64(values[i]);
                if (i == 0 && mask == 0L)
                    break;
                if ((bits & mask) == mask)
                {
                    results.Add(values[i]);
                    bits -= mask;
                }
            }
            if (bits != 0L)
                return Enumerable.Empty<Enum>();
            if (Convert.ToUInt64(value) != 0L)
                return results.Reverse<Enum>();
            if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
                return values.Take(1);
            return Enumerable.Empty<Enum>();
        }

        private static IEnumerable<Enum> GetFlagValues(Type enumType)
        {
            ulong flag = 0x1;
            foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                if (bits == 0L)
                    //yield return value;
                    continue; // skip the zero value
                while (flag < bits) flag <<= 1;
                if (flag == bits)
                    yield return value;
            }
        }

    }
}
