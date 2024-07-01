using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class ColliderExtensions
    {
        public static T GetComponentFromCollider<T>(this Collider collider)
        {
            if (collider == null)
                return default;
            // Get component in parent of the collider & also in his children

            T component = collider.GetComponentInParent<T>();
            if (component == null)
            {
                component = collider.GetComponentInChildren<T>();
            }

            return component;
        }
        
        public static bool TryGetComponentFromCollider<T>(this Collider collider, out T component)
        {
            component = GetComponentFromCollider<T>(collider);
            if (component == null)
                return false;
            return true;
        }

        public static IList<bool> GetCollidersEnablementState(this GameObject obj)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            List<bool> ret = new List<bool>();
            for (int i = 0; i < colliders.Length; i++)
                ret.Add(colliders[i].enabled);
            return ret;
        }

        public static void DisableAllColliders(this GameObject obj)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++)
                colliders[i].enabled = false;
        }

        public static void SetCollidersEnablementState(this GameObject obj, IList<bool> enablement)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            int max = Mathf.Max(colliders.Length, enablement.Count);
            for (int i = 0; i < max; i++)
                colliders[i].enabled = enablement[i];
        }

        public static void DisableAllShadows(this GameObject obj)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        public static bool IsBoxSphereCapsule(this Collider collider)
        {
            return collider is BoxCollider || collider is SphereCollider || collider is CapsuleCollider;
        }
    }
}
