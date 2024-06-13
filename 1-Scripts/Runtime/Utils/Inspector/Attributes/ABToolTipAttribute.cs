using UnityEngine;

namespace AnkleBreaker.Inspector
{
    public class ABToolTipAttribute : PropertyAttribute
    {
        public string TooltipMemberName { get; }

        public ABToolTipAttribute(string tooltipMemberName)
        {
            TooltipMemberName = tooltipMemberName;
        }
    }
}