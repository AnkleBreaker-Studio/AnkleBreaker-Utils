using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}