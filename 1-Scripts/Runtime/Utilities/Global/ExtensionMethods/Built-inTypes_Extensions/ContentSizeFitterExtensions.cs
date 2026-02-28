using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class ContentSizeFitterExtensions
    {
        public static void ForceRefresh(this ContentSizeFitter csf)
        {
            if (csf == null) return;
            IEnumerator Routine()
            {
                yield return new WaitForEndOfFrame();
                csf.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                yield return new WaitForEndOfFrame();
                csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            csf.StartCoroutine(Routine());
        }
        
        /// <summary>
        /// Forces a refresh on all child ContentSizeFitter components.
        /// </summary>
        /// <param name="csf">The parent ContentSizeFitter whose children to refresh.</param>
        /// <param name="inReverseOrder">If true, refreshes children in reverse order (leaf-first).</param>
        public static void ForceRefreshAllChildren(this ContentSizeFitter csf, bool inReverseOrder)
        {
            if (csf == null) return;
            
            var childFitters = csf.GetComponentsInChildren<ContentSizeFitter>(true);
            IEnumerator Routine()
            {
                if (inReverseOrder)
                {
                    for (int i = childFitters.Length - 1; i >= 0; i--)
                    {
                        if (childFitters[i] == null) continue;
                        yield return new WaitForEndOfFrame();
                        childFitters[i].verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                        yield return new WaitForEndOfFrame();
                        childFitters[i].verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    }
                }
                else
                {
                    for (int i = 0; i < childFitters.Length; i++)
                    {
                        if (childFitters[i] == null) continue;
                        yield return new WaitForEndOfFrame();
                        childFitters[i].verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                        yield return new WaitForEndOfFrame();
                        childFitters[i].verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    }
                }
            }
            csf.StartCoroutine(Routine());
        }
    }
}
