using System;

#if ODIN_INSPECTOR
using BaseAttribute = System.Attribute;
#else
using BaseAttribute = UnityEngine.PropertyAttribute;
#endif

namespace AnkleBreaker.Utils.Inspector
{
    public class AutoCompleteTextAttribute : BaseAttribute
    {
        public string[] Keys;
        public string[] ToolTips;
        
        /// <summary>
        ///   <para>The minimum amount of lines the text area will use.</para>
        /// </summary>
        public readonly int MinLines;
        /// <summary>
        ///   <para>The maximum amount of lines the text area can show before it starts using a scrollbar.</para>
        /// </summary>
        public readonly int MaxLines;

        public AutoCompleteTextAttribute(string[] keys, string[] toolTips = null,int minLines = 1, int maxLines = 1)
        {
            Keys = keys;
            ToolTips = toolTips ?? new string[keys.Length];
            MinLines = minLines;
            MaxLines = maxLines;
        }
    }
}
