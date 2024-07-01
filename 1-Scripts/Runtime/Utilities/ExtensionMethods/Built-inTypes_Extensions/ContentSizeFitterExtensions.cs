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
        
        public static void ForceRefreshAllChildren(this ContentSizeFitter csf, bool inReverseOrder)
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
    }
}
