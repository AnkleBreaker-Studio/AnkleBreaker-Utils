using System;
using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.Utils.Utils.EditorWindows.Dialog
{
    public class TextInputDialogWindow : EditorWindow
    {
        private string _enterTextMessage;
        private string _inputText = "";

        private Action<string> _onValidateInput;
        private Action _onCancelInput;

        public static void ShowDialog(string title, string enterTextMessage, Action<string> onValidate, Action onCancel, string defaultText = "")
        {
            TextInputDialogWindow textInputDialogWindow = GetWindow<TextInputDialogWindow>(true, title);
            textInputDialogWindow._onValidateInput = onValidate;
            textInputDialogWindow._onCancelInput = onCancel;
            textInputDialogWindow._enterTextMessage = enterTextMessage;
            textInputDialogWindow._inputText = defaultText;
            textInputDialogWindow.minSize = new Vector2(250, 100);
            textInputDialogWindow.maxSize = new Vector2(400, 120);
        }

        private void OnGUI()
        {
            GUILayout.Label(_enterTextMessage, EditorStyles.boldLabel);
            _inputText = EditorGUILayout.TextField(_inputText);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("OK"))
            {
                Action<string> onValidate = _onValidateInput;
                Close();
                
                // delaying call to make sure the action is called
                // after the dialog close
                EditorApplication.delayCall += () =>
                {
                    onValidate?.Invoke(_inputText);
                };
            }

            if (GUILayout.Button("Cancel"))
            {
                Action onCancelInput = _onCancelInput;
                Close();
                
                // delaying call to make sure the action is called
                // after the dialog close
                EditorApplication.delayCall += () =>
                {
                    onCancelInput?.Invoke();
                };
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            _onValidateInput = null;
            _onCancelInput = null;
        }
    }
}