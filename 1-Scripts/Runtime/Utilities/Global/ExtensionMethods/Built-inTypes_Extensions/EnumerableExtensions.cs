using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
	public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns whether the enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <returns>Whether the enumerable is null or empty.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
    }
}