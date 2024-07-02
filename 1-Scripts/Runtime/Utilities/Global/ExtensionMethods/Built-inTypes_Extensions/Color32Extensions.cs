using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class Color32Extensions
    {
        public static uint Color32ToUInt(this Color32 color)
        {
            return ((uint)color.r << 24) | ((uint)color.g << 16) | ((uint)color.b << 8) | color.a;
        }
        
        public static bool Equals(this Color32 colorA, Color32 colorB, bool checkAlpha = true)
        {
            return colorA.r == colorB.r
                && colorA.g == colorB.g
                && colorA.b == colorB.b
                && (!checkAlpha || colorA.a == colorB.a);
        }
    }
}