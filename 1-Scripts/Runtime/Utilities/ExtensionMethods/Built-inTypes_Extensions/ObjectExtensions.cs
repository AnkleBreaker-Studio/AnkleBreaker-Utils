using System;
using UnityEngine;
#if NEWTONSOFT_JSON
using Newtonsoft.Json;
#endif

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class ObjectExtensions
    {
        public static bool IsNumberType(this object obj)
        {
            return IsIntegerType(obj) || IsFloatingPointType(obj);
        }

        public static bool IsIntegerType(this object obj)
        {
            if (obj is Type type)
            {
                return type == typeof(sbyte)
                       || type == typeof(byte)
                       || type == typeof(short)
                       || type == typeof(ushort)
                       || type == typeof(int)
                       || type == typeof(uint)
                       || type == typeof(long)
                       || type == typeof(ulong);
            }

            return obj is sbyte
                or byte
                or short
                or ushort
                or int
                or uint
                or long
                or ulong;
        }

        public static bool IsFloatingPointType(this object obj)
        {
            return obj is float
                or double
                or decimal;
        }
        
        public static GameObject GameObject(this object uo)
        {
            if (uo is GameObject gameObject)
            {
                return gameObject;
            }
            else if (uo is Component component)
            {
                return component.gameObject;
            }
            else
            {
                return null;
            }
        }
        
#if NEWTONSOFT_JSON
        public static string SerializeObjectToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
#endif
    }
}
