using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class FloatExtensions
    {
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax, Func<float, float> mappingFunction = null)
        {
            float normalizedValue = Mathf.InverseLerp(fromMin, fromMax, value);
            if (mappingFunction != null)
                normalizedValue = mappingFunction(normalizedValue);
            return Mathf.Lerp(toMin, toMax, normalizedValue);
        }
    }
}
