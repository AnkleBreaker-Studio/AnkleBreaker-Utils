using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class QueueExtensions 
    {
        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize)
        {
            List<T> ret = new List<T>();
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
                ret.Add(queue.Dequeue());
            return ret;
        }
    }
}
