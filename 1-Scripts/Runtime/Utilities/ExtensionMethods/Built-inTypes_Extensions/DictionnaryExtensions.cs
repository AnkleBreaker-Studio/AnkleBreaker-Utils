using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class DictionaryExtensions
    {
        public static TValue Find<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            source.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Try random int until it find one not currently used as key in this dictionary(source param)
        /// </summary>
        /// <param name="source">Dictionary used to get available key</param>
        /// <param name="allowNegativeNbr">True by default, meaning the min value is 0</param>
        /// <returns></returns>
        public static int GetRandomAvailableIntKey<TValue>(this Dictionary<int, TValue> source)
        {
            int result = 0;
            result.SetNewRandomInt(false);

            while (source.ContainsKey(result))
                result.SetNewRandomInt(false);
            return result;
        }
        
        /// <summary>
        /// Try random int until it find one not currently used as key in this dictionary(source param)
        /// </summary>
        /// <param name="source">Dictionary used to get available key</param>
        /// <param name="allowNegativeNbr">True by default, meaning the min value is 0</param>
        /// <returns></returns>
        public static bool TryAdd<TValue>(this Dictionary<int, TValue> source, TValue valueToAdd, out int key)
        {
            key = -1;
            if (valueToAdd is null) return false;

            key = source.GetRandomAvailableIntKey();
            source.Add(key, valueToAdd);
            return true;
        }

        public static bool DictionaryEqual<TKey, TValue1, TValue2>(this IDictionary<TKey, TValue1> dict1, IDictionary<TKey, TValue2> dict2, Func<TValue1, TValue2, bool> valueEqualityFunc)
        {
            if (valueEqualityFunc == null)
                throw new ArgumentNullException("valueEqualityFunc shouldn't be null");

            if (dict1 == dict2)
                return true;

            if (dict1 == null | dict2 == null)
                return false;

            if (dict1.Count != dict2.Count)
                return false;

            return dict1.All(kvp =>
            {
                TValue2 value2;
                return dict2.TryGetValue(kvp.Key, out value2) && valueEqualityFunc(kvp.Value, value2);
            });
        }

        #region Callback dictionary
        /// <summary>
        /// Try to find key then invoke callback with passed result
        /// </summary>
        /// <param name="callbackId">CallbackId (dic key)</param>
        /// <param name="removeCallback">TRUE : remove from the dic after the invoke</param>
        /// <returns>TRUE : callback invoked and removed</returns>
        public static bool TryInvokeCallback(this Dictionary<int, Action> source, int callbackId, bool removeCallback = true)
        {
            if (!source.ContainsKey(callbackId)) return false;
            
            source[callbackId]?.Invoke();
            if (removeCallback)
                source.Remove(callbackId);
            return true;
        }
        
        /// <summary>
        /// Try to find key then invoke callback with passed result
        /// </summary>
        /// <param name="callbackId">CallbackId (dic key)</param>
        /// <param name="result">Result of the callback (dic value)</param>
        /// <param name="removeCallback">TRUE : remove from the dic after the invoke</param>
        /// <returns>TRUE : callback invoked and removed</returns>
        public static bool TryInvokeCallback<T>(this Dictionary<int, Action<T>> source, int callbackId, T result, 
            bool removeCallback = true)
        {
            if (!source.ContainsKey(callbackId)) return false;
            
            source[callbackId]?.Invoke(result);
            if (removeCallback)
                source.Remove(callbackId);
            return true;
        }
        
        /// <summary>
        /// Try to find key then invoke callback with passed result
        /// </summary>
        /// <param name="callbackId">CallbackId (dic key)</param>
        /// <param name="result">Result of the callback (dic value)</param>
        /// <param name="removeCallback">TRUE : remove from the dic after the invoke</param>
        /// <returns>TRUE : callback invoked and removed</returns>
        public static bool TryInvokeCallback<T1, T2>(this Dictionary<int, Action<T1, T2>> source, int callbackId, 
            T1 value1, T2 value2, bool removeCallback = true)
        {
            if (!source.ContainsKey(callbackId)) return false;
            
            source[callbackId]?.Invoke(value1, value2);
            if (removeCallback)
                source.Remove(callbackId);
            return true;
        }
        #endregion
    }
}