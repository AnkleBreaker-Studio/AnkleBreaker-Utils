using UnityEngine.EventSystems;
using UnityEngine.UI;
#if TEXTMESHPRO
using TMPro;
#endif

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class EventSystemExtensions
    {
#if TEXTMESHPRO
        public static bool IsSelectingAnInputField(this EventSystem src)
        {
            if (src != null && src.currentSelectedGameObject)
                return src.currentSelectedGameObject.TryGetComponent(out InputField inputField)
                    || src.currentSelectedGameObject.TryGetComponent(out TMP_InputField tmpInputField);

            return false;
        }
#endif
    }
}