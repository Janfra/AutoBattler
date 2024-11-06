using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ShowScriptableObjectEditorAttribute))]
public class ShowScriptableObjectEditorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        object value = property.boxedValue;
        if (!(value is ScriptableObject))
        {
            EditorGUI.PropertyField(position, property, label);
        }
        
        EditorGUILayout.BeginHorizontal();
        EditorGUI.PropertyField(position, property, label);
        position.x -= 12;
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, "");
        EditorGUILayout.EndHorizontal();

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(property.displayName + " Inspector");
            Editor scriptableEditor = Editor.CreateEditor(value as ScriptableObject);
            scriptableEditor.OnInspectorGUI();
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space(5);

    }
}
