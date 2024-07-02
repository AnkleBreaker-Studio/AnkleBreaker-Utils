using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class RectTransformExtensions
    {
        public static void RebuildAllLayoutInChildren(this RectTransform srcRect)
        {
            // Check if srcRect have MonoBehavior
            var componentsInChildren = srcRect.GetComponentsInChildren<LayoutGroup>();

            foreach (var layoutGroup in componentsInChildren)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(srcRect.GetComponent<RectTransform>());
        }
        
        public static void RebuildAllLayoutInChildrenWithDelay(this RectTransform srcRect, MonoBehaviour mb,
            int nbrOffFrameToWait)
        {
            IEnumerator<RectTransform> RebuildAllLayoutsInChildrenCoroutine(Action<RectTransform> action, 
                RectTransform rect, int nbrOffFrameToWait)
            {
                while (nbrOffFrameToWait > 0)
                {
                    yield return null;
                    nbrOffFrameToWait--;
                }
                action.Invoke(rect);
            }
            
            mb.StartCoroutine(RebuildAllLayoutsInChildrenCoroutine(
                RebuildAllLayoutInChildren, srcRect, nbrOffFrameToWait));
        }
    }
}