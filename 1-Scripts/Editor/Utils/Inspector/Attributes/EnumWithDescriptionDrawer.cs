using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace AnkleBreaker.Utils.Inspector.Editor
{
    public class EnumWithDescriptionDrawer<TEnum> : OdinValueDrawer<TEnum> where TEnum : Enum
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            TEnum value = this.ValueEntry.SmartValue;

            // Fetch description from attribute
            string description = GetEnumDescription(value);

            // Draw the description info box above the enum dropdown if description exists
            if (!string.IsNullOrEmpty(description))
            {
                // Créez une zone d'affichage pour l'info box au-dessus
                GUILayout.BeginVertical();
                GUILayout.Space(5); // Un peu d'espace entre l'info box et l'élément enum
                SirenixEditorGUI.InfoMessageBox(description);
                GUILayout.EndVertical();
            }

            // Draw the actual enum dropdown
            var newValue = (TEnum)SirenixEditorFields.EnumDropdown(label, value);
            if (!value.Equals(newValue))
            {
                this.ValueEntry.SmartValue = newValue;
            }
        }

        private string GetEnumDescription(TEnum value)
        {
            var memberInfo = typeof(TEnum).GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                var attr = memberInfo[0].GetCustomAttribute<EnumDescriptionAttribute>();
                if (attr != null)
                {
                    return attr.Description;
                }
            }

            return null;
        }
    }
}