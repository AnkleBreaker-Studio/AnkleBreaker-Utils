using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class LayerMaskExtension
    {
        /// <summary>
        /// Extension method to check if a layer is in a layermask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        #if UNITY_EDITOR
        public static LayerMask AddLayer(this LayerMask mask, int layerToAdd)
        {
            // Get current layer mask value
            int currentLayerMaskValue = mask.value;

            // add a new layer activating the bit
            currentLayerMaskValue |= (1 << layerToAdd);

            return currentLayerMaskValue;
        }
        #endif
    }
}