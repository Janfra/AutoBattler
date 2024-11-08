using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace MyCustomEditor
{
    public class CustomGUIEditorWindow : ExtendedEditorWindow
    {
        public static void OpenWindow<T>(T windowTarget, string windowName) where T : Object
        {
            if (windowTarget == null)
            {
                Debug.LogError("Custom editor window target is null");
                return;
            }

            CustomGUIEditorWindow window = GetWindow<CustomGUIEditorWindow>(windowName);
            window.serializedObject = new SerializedObject(windowTarget);
            window.targetObject = windowTarget;
        }



        private void OnGUI()
        {
            if (serializedObject == null && targetObject != null)
            {
                serializedObject = new SerializedObject(targetObject);
            }

            Draw(serializedObject, true);
            ApplyChanges();
        }
    }
}
