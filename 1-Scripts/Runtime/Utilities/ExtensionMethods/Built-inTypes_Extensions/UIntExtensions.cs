using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class UIntExtensions
    {
        public static Color32 UIntToColor32(this uint value)
        {
            byte r = (byte)((value >> 24) & 0xFF);
            byte g = (byte)((value >> 16) & 0xFF);
            byte b = (byte)((value >> 8) & 0xFF);
            byte a = (byte)(value & 0xFF);

            return new Color32(r, g, b, a);
        }
    }
}