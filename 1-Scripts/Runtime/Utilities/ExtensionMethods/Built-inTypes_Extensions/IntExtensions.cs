using System;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods
{
    public static class IntExtensions
    {
        public static void SetNewRandomInt(this ref int src, bool allowNegative, int maxValue = int.MaxValue)
        {
            if (maxValue < 0)
                maxValue = Int32.MaxValue;

            src = UnityEngine.Random.Range(allowNegative ? int.MinValue : 0, maxValue);
        }

        public static void SetNewRandomInt(this ref int src, int? minValue = Int32.MinValue, int maxValue = int.MaxValue)
        {
            if (!minValue.HasValue)
            {
                minValue = 0;
                if (minValue.Value >= maxValue)
                {
                    minValue = Int32.MinValue;
                    Debug.LogWarning(
                        "Invalid Range when finding RandomInt : MinValue shouldn't be superior than maxValue!");
                }
            }

            src = UnityEngine.Random.Range(minValue.Value, maxValue);
        }
    }
}