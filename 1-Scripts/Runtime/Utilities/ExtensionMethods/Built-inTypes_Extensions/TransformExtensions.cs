using System;
using System.Collections.Generic;
using AnkleBreaker.Utils.TypeDefinitions;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform targetParent)
        {
            List<Transform> children = targetParent.GetAllChildren();

            foreach (Transform child in children)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(child.gameObject);
#else
                GameObject.Destroy(child.gameObject);
#endif
            }
        }

        public static void DestroyChildrenOfTag(this Transform targetParent, string tagName)
        {
            List<Transform> children = targetParent.GetAllChildren();

            foreach (Transform child in children)
            {
                if (child.CompareTag(tagName))
                {
#if UNITY_EDITOR
                    GameObject.DestroyImmediate(child.gameObject);
#else
                    GameObject.Destroy(child.gameObject);
#endif
                }
            }
        }

        public static List<Transform> GetAllChildren(this Transform parent)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                list.Add(parent.GetChild(i));
            }

            return list;
        }

        public static Transform GetChildWithTag(this Transform parent, string tagName)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.CompareTag(tagName))
                    return child;
            }
            return null;
        }

        public static Transform GetChildWithTagRecursive(this Transform parent, string tagName)
        {
            if (parent.CompareTag(tagName))
                    return parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.CompareTag(tagName))
                    return child;
                child = child.GetChildWithTagRecursive(tagName);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }

        public static Transform FindChildRecursiveIgnoreCase(this Transform parent, string childName)
        {
            if (parent.name.Equals(childName, StringComparison.InvariantCultureIgnoreCase))
                return parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                child = child.FindChildRecursiveIgnoreCase(childName);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }

        public static PositionRotation GetPositionRotation(this Transform transform, bool local = false)
        {
            PositionRotation positionRotation = new PositionRotation();
            if (local)
            {
                positionRotation.Position = transform.localPosition;
                positionRotation.Rotation = transform.localRotation;
            }
            else
            {
                positionRotation.Position = transform.position;
                positionRotation.Rotation = transform.rotation;
            }
            return positionRotation;
        }

        public static void SetPositionRotation(this Transform transform, PositionRotation positionRotation, bool local = false)
        {
            if (local)
            {
                transform.localPosition = positionRotation.Position;
                transform.localRotation = positionRotation.Rotation;
            }
            else
            {
                transform.position = positionRotation.Position;
                transform.rotation = positionRotation.Rotation;
            }
        }

        public static string GetHierarchyPath(this Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }

        public static void ApplyLayerToAllChild(this Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            foreach(Transform child in transform)
            {
                ApplyLayerToAllChild(child, layer);
            }
        }
        
        public static void ApplyTagToAllChild(this Transform transform, string tag)
        {
            transform.gameObject.tag = tag;
            foreach(Transform child in transform)
            {
                ApplyTagToAllChild(child, tag);
            }
        }
        
        public static Bounds TransformBounds(this Transform _transform, Bounds _localBounds)
        {
            var center = _transform.TransformPoint(_localBounds.center);

            // transform the local extents' axes
            var extents = _localBounds.extents;
            var axisX = _transform.TransformVector(extents.x, 0, 0);
            var axisY = _transform.TransformVector(0, extents.y, 0);
            var axisZ = _transform.TransformVector(0, 0, extents.z);

            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }
        
        public static void RemoveChildren (this Transform tfm)
        {
            for (int i=tfm.childCount-1; i>=0; i--)
            {
                Transform child = tfm.GetChild(i);
				
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                    GameObject.DestroyImmediate(child.gameObject);
                else
#endif
                    GameObject.Destroy(child.gameObject);
            }
        }
        
        public static Transform FindChildRecursive (this Transform tfm, string name)
        {
            int numChildren = tfm.childCount;

            for (int i=0; i<numChildren; i++)
                if (tfm.GetChild(i).name == name) return tfm.GetChild(i);

            for (int i=0; i<numChildren; i++)
            {
                Transform result = tfm.GetChild(i).FindChildRecursive(name);
                if (result != null) return result;
            }

            return null;
        }
    }
}