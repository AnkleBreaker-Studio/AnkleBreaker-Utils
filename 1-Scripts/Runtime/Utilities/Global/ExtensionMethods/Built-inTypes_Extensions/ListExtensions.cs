using System.Collections.Generic;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class ListExtensions
    {
        public static bool IsInRange<T>(this List<T> src, int index)
        {
            int count = src.Count;
            return count > 0 && index >= 0 && index < count;
        }

        public static void SetActiveObjects<T>(this List<T> src, bool toEnable) where T : Object
        {
            if (src.Count == 0) return;

            foreach (T obj in src)
                obj.GameObject()?.SetActive(toEnable);
        }

        // Browse the list (must be sorted) and compare each value with the passed one. The method will output the index of the value which is immediately above the passed one and below the next index value
        // Note: if value is below any list ones, the method returns -1
        // Note: if value if above any list ones, the method returns list count-1
        public static int GetIndexForValue(this List<float> src, float toFind) 
        {
            if (src.Count == 0)
                return -1;

            for (int i = 0; i < src.Count - 1; i++)
            {
                if (toFind > (float)src[i] && toFind < (float)src[i + 1])
                    return i;
            }

            if (toFind < src[0])
                return -1;
            else
                return src.Count - 1;
        }
        
        /// <summary>
        /// Removes null entries from a list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to remove null entries from.</param>
        public static void RemoveNullEntries<T>(this IList<T> list) where T : class
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (Equals(list[i], null))
                    list.RemoveAt(i);
        }

        /// <summary>
        /// Removes default values from a list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to remove default values from.</param>
        public static void RemoveDefaultValues<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (Equals(default(T), list[i]))
                    list.RemoveAt(i);
        }
    }
}
