using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class GameObjectExtension
    {
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
                child.gameObject.SetLayerRecursively(layer);
        }

        public static Bounds CalculateObjectBounds(this GameObject obj, List<Collider> collidersToExclude = null)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);

            if (collidersToExclude != null)
            {
                colliders = colliders.Where(x => !collidersToExclude.Contains(x)).ToArray();
            }

            if (colliders.Length > 0)
            {
                Bounds bounds = colliders[0].bounds;
                for (int i = 1; i < colliders.Length; i++)
                {
                    bounds.Encapsulate(colliders[i].bounds);
                }
                return bounds;
            }
            else
            {
                // If there are no colliders, use a default bounds
                return new Bounds(obj.transform.position, Vector3.zero);
            }
        }
    }
}