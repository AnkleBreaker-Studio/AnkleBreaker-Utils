using UnityEngine;

namespace AnkleBreaker.Inspector
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class LabelTextAttribute : PropertyAttribute
    {
        public string DisplayName { get; }

        public LabelTextAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}