using System;

namespace AnkleBreaker.Utils
{
    public static class FlagsHelper
    {
        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static bool IsAllSet<T>(T flags, params T[] flagToCheck) where T : struct
        {
            int flagsValue = (int)(object)flags;
            foreach (T flag in flagToCheck)
            {
                int flagValue = (int)(object)flag;
                if ((flagsValue & flagValue) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAnySet<T>(T flags, params T[] flagToCheck) where T : struct
        {
            int flagsValue = (int)(object)flags;
            foreach (T flag in flagToCheck)
            {
                int flagValue = (int)(object)flag;
                if ((flagsValue & flagValue) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }

        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }

        public static string Print<T>(T flag) where T : struct
        {
            int x = (int)(object)flag;
            string result = "'";
            int i = 1;
            int c = 0;
            while (i < (1 << 30))
            {
                if ((i & x) != 0)
                {
                    if (result.Length > 2)
                        result += " ";
                    result += Enum.GetName(typeof(T), i);
                }

                c++;
                i <<= 1;
            }

            return result + $"'({x})";
        }

        public static bool IsAllSplitAreSet<T>(T flags1, T flags2) where T : struct
        {
            int flags1Value = (int)(object)flags1;
            int flags2Value = (int)(object)flags2;

            int i = 1;
            while (i < (1 << 30))
            {
                if ((i & flags1Value) != 0)
                {
                    if ((i & flags2Value) == 0)
                    {
                        return false;
                    }
                }

                i <<= 1;
            }

            return true;
        }
    }
}
